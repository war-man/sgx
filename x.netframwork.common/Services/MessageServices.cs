﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Common.Utilities;
using Data;
using MimeKit;
using MimeKit.Text;
using Models;
using MongoDB.Driver;

namespace Services
{
    public class AuthMessageSender
    {
        public AuthMessageSender()
        {
        }

        public void SendEmail(EmailMessage emailMessage)
        {
            #region Connection
            var connectString = "mongodb://localhost:27017";
            MongoDBContext.ConnectionString = connectString;
            MongoDBContext.DatabaseName = "tribat";
            MongoDBContext.IsSSL = true;
            MongoDBContext dbContext = new MongoDBContext();
            #endregion

            var message = new MimeMessage
            {
                Subject = emailMessage.Subject,
                Body = new TextPart(TextFormat.Html)
                {
                    Text = emailMessage.BodyContent
                }
            };

            // Sometime null from, set default
            if (emailMessage.FromAddresses == null || emailMessage.FromAddresses.Count == 0)
            {
                emailMessage.FromAddresses = new List<EmailAddress>
                    {
                        new EmailAddress { Name = Constants.System.emailHrName, Address = Constants.System.emailHr, Pwd = Constants.System.emailHrPwd}
                    };
            }
            message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));

            // Check toemail
            var newToList = new List<EmailAddress>();
            if (emailMessage.ToAddresses != null && emailMessage.ToAddresses.Count > 0)
            {
                foreach(var item in emailMessage.ToAddresses)
                {
                    if (IsValidEmail(item.Address))
                    {
                        newToList.Add(item);
                    }
                    else
                    {
                        // Chỉ gửi cho email sai
                        var toError = new List<EmailAddress>
                        {
                            item
                        };
                        var errorEmail = new ScheduleEmail
                        {
                            From = emailMessage.FromAddresses,
                            To = toError,
                            CC = emailMessage.CCAddresses,
                            BCC = emailMessage.BCCAddresses,
                            Type = emailMessage.Type,
                            Title = message.Subject,
                            Content = emailMessage.BodyContent,
                            Status = 2,
                            Error = "Sai định dạng mail",
                            ErrorCount = 0
                        };
                        dbContext.ScheduleEmails.InsertOne(errorEmail);
                        SendMailSupport(errorEmail.Id);
                    }
                }
            }

            if (newToList != null && newToList.Count > 0)
            {
                message.To.AddRange(newToList.Select(x => new MailboxAddress(x.Name, x.Address)));

                if (emailMessage.CCAddresses != null && emailMessage.CCAddresses.Count > 0)
                {
                    message.Cc.AddRange(emailMessage.CCAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
                }
                if (emailMessage.BCCAddresses != null && emailMessage.BCCAddresses.Count > 0)
                {
                    message.Bcc.AddRange(emailMessage.BCCAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
                }

                #region Add to schedule
                var scheduleEmail = new ScheduleEmail
                {
                    From = emailMessage.FromAddresses,
                    To = emailMessage.ToAddresses,
                    CC = emailMessage.CCAddresses,
                    BCC = emailMessage.BCCAddresses,
                    Type = emailMessage.Type,
                    Title = message.Subject,
                    Content = emailMessage.BodyContent
                };
                dbContext.ScheduleEmails.InsertOne(scheduleEmail);
                #endregion
                var isEmailSent = 0;
                var error = string.Empty;
                try
                {
                    using (var emailClient = new MailKit.Net.Smtp.SmtpClient())
                    {
                        //The last parameter here is to use SSL (Which you should!)
                        emailClient.Connect(emailMessage.FromAddresses.First().Address, 465, true);

                        //Remove any OAuth functionality as we won't be using it. 
                        emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                        emailClient.Authenticate(emailMessage.FromAddresses.First().Address, emailMessage.FromAddresses.First().Pwd);

                        emailClient.Send(message);
                        isEmailSent = 1;

                        emailClient.Disconnect(true);
                        #region Update status
                        var filter = Builders<ScheduleEmail>.Filter.Eq(m => m.Id, scheduleEmail.Id);
                        var update = Builders<ScheduleEmail>.Update
                            .Set(m => m.Status, isEmailSent)
                            .Set(m => m.UpdatedOn, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"));
                        dbContext.ScheduleEmails.UpdateOne(filter, update);
                        #endregion
                    }
                }
                catch (Exception ex)
                {
                    isEmailSent = 2;
                    error = ex.Message;
                    #region Update status
                    var filter = Builders<ScheduleEmail>.Filter.Eq(m => m.Id, scheduleEmail.Id);
                    var update = Builders<ScheduleEmail>.Update
                        .Set(m => m.Status, isEmailSent)
                        .Set(m => m.Error, error)
                        .Inc(m => m.ErrorCount, 1)
                        .Set(m => m.UpdatedOn, DateTime.Now.ToString("MM/dd/yyyy HH:mm:ss.fff"));
                    dbContext.ScheduleEmails.UpdateOne(filter, update);
                    #endregion
                    SendMailSupport(scheduleEmail.Id);
                }
            }
        }

        private void SendMailSupport(string id)
        {
            #region Connection
            var connectString = "mongodb://localhost:27017";
            MongoDBContext.ConnectionString = connectString;
            MongoDBContext.DatabaseName = "tribat";
            MongoDBContext.IsSSL = true;
            MongoDBContext dbContext = new MongoDBContext();
            #endregion

            var errorItem = dbContext.ScheduleEmails.Find(m => m.Id.Equals(id)).FirstOrDefault();
            var subject = "Gửi email lỗi " + errorItem.Title;
            var froms = new List<EmailAddress>
                        {
                            new EmailAddress {
                                Name = Constants.System.emailHrName ,
                                Address = Constants.System.emailHr,
                                Pwd = Constants.System.emailHrPwd
                            }
                        };
            var tos = new List<EmailAddress>
                            {
                                new EmailAddress { Name = "Trần Minh Xuân", Address = "xuan.tm@tribat.vn" }
                            };
            var pathToFile = Environment.CurrentDirectory + "/Templates/Error.html";
            var bodyBuilder = new BodyBuilder();
            using (StreamReader SourceReader = File.OpenText(pathToFile))
            {
                bodyBuilder.HtmlBody = SourceReader.ReadToEnd();
            }
            string messageBody = string.Format(bodyBuilder.HtmlBody,
                subject,
                errorItem.Id,
                errorItem.UpdatedOn,
                errorItem.Error,
                errorItem.Type,
                errorItem.Content);
            var emailMessage = new EmailMessage()
            {
                FromAddresses = froms,
                ToAddresses = tos,
                Subject = subject,
                BodyContent = messageBody
            };

            var message = new MimeMessage();
            message.From.AddRange(emailMessage.FromAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
            message.To.AddRange(emailMessage.ToAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
            if (emailMessage.CCAddresses != null && emailMessage.CCAddresses.Count > 0)
            {
                message.Cc.AddRange(emailMessage.CCAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
            }
            if (emailMessage.BCCAddresses != null && emailMessage.BCCAddresses.Count > 0)
            {
                message.Bcc.AddRange(emailMessage.BCCAddresses.Select(x => new MailboxAddress(x.Name, x.Address)));
            }
            message.Subject = emailMessage.Subject;
            message.Body = new TextPart(TextFormat.Html)
            {
                Text = emailMessage.BodyContent
            };
            try
            {
                using (var emailClient = new MailKit.Net.Smtp.SmtpClient())
                {
                    //The last parameter here is to use SSL (Which you should!)
                    emailClient.Connect(emailMessage.FromAddresses.First().Address, 465, true);

                    //Remove any OAuth functionality as we won't be using it. 
                    emailClient.AuthenticationMechanisms.Remove("XOAUTH2");

                    emailClient.Authenticate(emailMessage.FromAddresses.First().Address, emailMessage.FromAddresses.First().Pwd);

                    emailClient.Send(message);

                    emailClient.Disconnect(true);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }
    }
}
