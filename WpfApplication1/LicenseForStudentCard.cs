using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Security.Cryptography;


namespace WpfApplication1
{
    public class LicenseForStudentCard
    {
        public LicenseForStudentCard() { }
        public string Name { get; private set; }
        public string nameForWelcome;
        public DateTime StartDate { get; private set; }
        public DateTime UpdateTo { get; private set; }

        public void Verify()
        {
            string File = "license.xml";
            if (!System.IO.File.Exists(File))
            {
                throw new ApplicationException("Отсутствует лицензия!");
            }
            XmlDocument doc = new XmlDocument();
            doc.Load(File);
            string sig1;
            string Signature;
            try
            {
                string Name = doc.ChildNodes[0].SelectSingleNode(@"/license/Name", null).InnerText;
                nameForWelcome = Name;
                string StartDate = doc.ChildNodes[0].SelectSingleNode(@"/license/Date", null).InnerText;
                string UpdateTo = doc.ChildNodes[0].SelectSingleNode(@"/license/UpdateTo", null).InnerText;
                Signature = doc.ChildNodes[0].SelectSingleNode(@"/license/Signature", null).InnerText;
                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] data = System.Text.Encoding.UTF8.GetBytes(Name + StartDate + UpdateTo + "SomePasswordKey");
                byte[] hash = md5.ComputeHash(data);
                sig1 = Convert.ToBase64String(hash);
                this.Name = Name;
                this.StartDate = Convert.ToDateTime(StartDate);
                this.UpdateTo = Convert.ToDateTime(UpdateTo);
            }
            catch (Exception)
            {

                throw new ApplicationException("Отсутствует лицензия!");
            }

            if (sig1 != Signature)
            {
                throw new ApplicationException("Отсутствует лицензия!");

            }
            if (DateTime.Now< this.StartDate)
            {
                throw new ApplicationException(string.Format("Отсутствует лицензия!", StartDate.ToShortDateString()));
            }
            if (DateTime.Now > this.UpdateTo)
            {
                throw new ApplicationException(string.Format("Отсутствует лицензия!", UpdateTo.ToShortDateString()));
            }
        }
    }
}