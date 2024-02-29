using Company.DAL.models;

namespace company.PL.Helper
{
    public interface IMailSetting
    {
        public void SendMail(Email email);
    }
}
