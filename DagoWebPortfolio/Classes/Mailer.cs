using QCBDManagementCommon.Classes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace DagoWebPortfolio.Classes
{
    public class Mailer
    {
        MailMessage mail;
        private SmtpClient _client;

        public string Login { get; set; }
        public string Password { get; set; }
        public string From { get; set; }
        public string Subject { get; set; }
        public string FromName { get; set; }
        public string Body { get; set; }
        public string Host { get; set; }
        public bool IsHtml { get; set; }
        public bool IsDebug { get; set; }

        public Mailer(string smtpHost)
        {
            Host = smtpHost;
            mail = new MailMessage();
            _client = new SmtpClient(smtpHost);// ("smtp.gmail.com", 587);
            
        }

        public void initialize()
        {
            mail.Subject = Subject;
            if (!string.IsNullOrEmpty(FromName))
                mail.From = new MailAddress(From, FromName);
            else
                mail.From = new MailAddress(From);
            mail.Body = Body;
            mail.IsBodyHtml = IsHtml;

            //_client.Host = "smtp.gmail.com";
            _client.UseDefaultCredentials = false;
            System.Net.NetworkCredential credential = new System.Net.NetworkCredential(Login, Password);// ("sisi.bahilo@gmail.com", "bahilo225");
            _client.Credentials = credential;
            //_client.Port = 587;
            //_client.EnableSsl = true;
            _client.DeliveryMethod = SmtpDeliveryMethod.Network;

        }



        public bool send()
        {
            try
            {
                _client.Send(mail);
            }
            catch (Exception ex)
            {
                Log.write(ex.Message, "ERR");
                return false;
            }
            return true;
        }

        public void addAnAddress(Dictionary<string, List<string>> addressesList)
        {
            foreach (var addresses in addressesList)
            {
                if (addresses.Key == "To")
                {
                    foreach (var address in addresses.Value)
                    {
                        mail.To.Add(address);
                    }
                }

                if (addresses.Key == "Reply-To")
                {
                    foreach (var address in addresses.Value)
                    {
                        mail.ReplyToList.Add(address);
                    }
                }

                if (addresses.Key == "Cc")
                {
                    foreach (var address in addresses.Value)
                    {
                        mail.CC.Add(address);
                    }
                }

                if (addresses.Key == "Bcc")
                {
                    foreach (var address in addresses.Value)
                    {
                        mail.Bcc.Add(address);
                    }
                }
            }
        }

        public void addAttachment(List<string> fileNameFullPathList)
        {
            foreach (var attach in fileNameFullPathList){
				mail.Attachments.Add(new Attachment(attach));
            }
        }
        



    }
}
