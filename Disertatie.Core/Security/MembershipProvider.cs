using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Security;
using System.Configuration.Provider;
using System.Security.Cryptography;
using System.Collections.Specialized;
using Disertatie.Core.Models;
using Disertatie.Core.Repositories;

namespace Disertatie.Core.Security
{
    public class MembershipProvider : System.Web.Security.MembershipProvider
    {
        #region Fields
        private string _applicationName;
        private bool _enablePasswordReset;
        private bool _enablePasswordRetrieval = false;
        private int _maxInvalidPasswordAttempts;
        private int _minRequiredNonAlphanumericCharacters;
        private int _minRequiredPasswordLength;
        private int _passwordAttemptWindow;
        private bool _requiresQuestionAndAnswer;
        private bool _requiresUniqueEmail = false;
        private MembershipPasswordFormat _passwordFormat = MembershipPasswordFormat.Hashed;
        private string _passwordStrengthRegularExpression;

        #endregion
       
        #region Properties

        public Disertatie.Core.Repositories.UtilizatorRepository UtilizatorRepository { get; set; }
        public override string ApplicationName { get { return _applicationName; } set { ;} }
        public override bool EnablePasswordReset { get { return _enablePasswordReset; } }
        public override bool EnablePasswordRetrieval { get { return _enablePasswordRetrieval; } }
        public override int MaxInvalidPasswordAttempts { get { return _maxInvalidPasswordAttempts; } }
        public override int MinRequiredNonAlphanumericCharacters { get { return _minRequiredNonAlphanumericCharacters; } }
        public override int MinRequiredPasswordLength { get { return _minRequiredPasswordLength; } }
        public override int PasswordAttemptWindow { get { return _passwordAttemptWindow; } }
        public override bool RequiresQuestionAndAnswer { get { return _requiresQuestionAndAnswer; } }
        public override bool RequiresUniqueEmail { get { return _requiresUniqueEmail; } }
        public override MembershipPasswordFormat PasswordFormat { get { return _passwordFormat; } }
        public override string PasswordStrengthRegularExpression { get { return _passwordStrengthRegularExpression; } }

        #endregion
        private string GetConfigValue(string configValue, string defaultValue)
        {
            if (string.IsNullOrEmpty(configValue))
                return defaultValue;

            return configValue;
        }
        public override void Initialize(string name, NameValueCollection config)
        {
            base.Initialize(name, config);

            if (config == null)
                throw new ArgumentNullException("config");

            if (name == null || name.Length == 0)
                name = "MembershipProvider";

            if (String.IsNullOrEmpty(config["description"]))
            {
                config.Remove("description");
                config.Add("description", "Custom Membership Provider");
            }

            //base.Initialize(name, config);

            _applicationName = GetConfigValue(config["applicationName"], System.Web.Hosting.HostingEnvironment.ApplicationVirtualPath);
            _maxInvalidPasswordAttempts = Convert.ToInt32(GetConfigValue(config["maxInvalidPasswordAttempts"], "5"));
            _passwordAttemptWindow = Convert.ToInt32(GetConfigValue(config["passwordAttemptWindow"], "10"));
            _minRequiredNonAlphanumericCharacters = Convert.ToInt32(GetConfigValue(config["minRequiredNonalphanumericCharacters"], "1"));
            _minRequiredPasswordLength = Convert.ToInt32(GetConfigValue(config["minRequiredPasswordLength"], "6"));
            _enablePasswordReset = Convert.ToBoolean(GetConfigValue(config["enablePasswordReset"], "true"));
            _passwordStrengthRegularExpression = Convert.ToString(GetConfigValue(config["passwordStrengthRegularExpression"], ""));

        }
        #region Membership Provider Overrides

        public override bool ChangePassword(string username, string oldPassword, string newPassword)
        {
            if (!ValidateUser(username, oldPassword))
            {
                return false;
            }
            ValidateNewPasswordFormat(username, newPassword);
            try
            {
                ChangeUserPassword(username, newPassword);
            }
            catch (Exception ex)
            {
                throw new MemberAccessException("Eroare la procesarea datelor de autentificare", ex);
            }
            return true;
        }

        public override bool ChangePasswordQuestionAndAnswer(string username, string password, string newPasswordQuestion, string newPasswordAnswer)
        {
            if (ValidateUser(username, password))
            {
                try
                {
                    UpdateUserQuestionAndAnswer(username, newPasswordQuestion, newPasswordAnswer);
                }
                catch (Exception ex)
                {
                    throw new MemberAccessException("Eroare la procesarea datelor de autentificare - " + ex.Message);
                }
                return true;
            }
            return false;
        }

        public override MembershipUser CreateUser(string username, string password, string email, string passwordQuestion, string passwordAnswer, bool isApproved, object providerUserKey, out System.Web.Security.MembershipCreateStatus status)
        {
            try
            {
                ValidateNewPasswordFormat(username, password);
            }
            catch (Exception)
            {
                status = MembershipCreateStatus.InvalidPassword;
                return null;
            }
            if (DuplicatesEmailAddress(email))
            {
                status = MembershipCreateStatus.DuplicateEmail;
                return null;
            }
            MembershipUser membershipUser = GetUser(username, false);
            if (membershipUser == null)
            {
                var u = new Utilizator(username, email)
                {
                    NumeAplicatie = this.ApplicationName,
                    Parola =EncodePassword( password),
                    IntrebareParola = passwordQuestion,
                    RaspunsParola =passwordAnswer,
                    Aprobat = isApproved,
                     
                };
                try
                {
                    UtilizatorRepository = new UtilizatorRepository();
                    UtilizatorRepository.Save(u);
                    UtilizatorRepository.FlushSession();
                    status = MembershipCreateStatus.Success;
                }
                catch (Exception ex)
                {
                    throw new MemberAccessException("Eroare la procesarea datelor de autentificare", ex);
                }
                return GetUser(username, false);
            }
            else
            {
                status = MembershipCreateStatus.DuplicateUserName;
            }
            return null;
        }

        public override bool DeleteUser(string username, bool deleteAllRelatedData)
        {
            try
            {
                DeleteUserDataByName(username);
            }
            catch (Exception ex)
            {
                throw new MemberAccessException("Eroare la procesarea datelor de autentificare", ex);
            }
            return true;
        }

        public override MembershipUserCollection FindUsersByEmail(string emailToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            try
            {
                var users = UtilizatorRepository.FindUsersByEmail(emailToMatch, pageIndex, pageSize, this.ApplicationName);
                totalRecords = users.Count;
                return MapUsers(users);
            }
            catch (Exception ex)
            {
                throw new MemberAccessException("Eroare la procesarea datelor de autentificare", ex);
            }
        }

        public override MembershipUserCollection FindUsersByName(string usernameToMatch, int pageIndex, int pageSize, out int totalRecords)
        {
            try
            {
                var users = UtilizatorRepository.FindUsersByName(usernameToMatch, pageIndex, pageSize, this.ApplicationName);
                totalRecords = users.Count;
                return MapUsers(users);
            }
            catch (Exception ex)
            {
                throw new MemberAccessException("Eroare la procesarea datelor de autentificare", ex);
            }
        }

        public override MembershipUserCollection GetAllUsers(int pageIndex, int pageSize, out int totalRecords)
        {
            try
            {
                var users = UtilizatorRepository.GetAllUsers(pageIndex, pageSize, this.ApplicationName);
                totalRecords = users.Count;
                return MapUsers(users);
            }
            catch (Exception ex)
            {
                throw new MemberAccessException("Eroare la procesarea datelor de autentificare", ex);
            }
        }

        public override int GetNumberOfUsersOnline()
        {
            var compareTime = DateTime.Now.AddMinutes(-Membership.UserIsOnlineTimeWindow); // subtraction

            try
            {
                return UtilizatorRepository.GetNumberOfUsersOnline(compareTime, this.ApplicationName);
            }
            catch (Exception ex)
            {
                throw new MemberAccessException("Eroare la procesarea datelor de autentificare", ex);
            }
        }

        public override string GetPassword(string username, string answer)
        {
            if (EnablePasswordRetrieval)
            {
                if (PasswordFormat == MembershipPasswordFormat.Hashed)
                {
                    throw new ProviderException("Nu se poate recupera parola.");
                }
                return RetrievePasswordFromDatabase(username, answer);
            }
            throw new ProviderException("Nu a fost activata optiunea de recuperare a parolei");
        }

        public override MembershipUser GetUser(string username, bool userIsOnline)
        {
            try
            {

                var t = new Disertatie.Core.Repositories.UtilizatorRepository();
                // User u = UserRepository.GetUserByName(username, this.ApplicationName);
                Utilizator u = t.GetUserByName(username, this.ApplicationName);

                if (u != null)
                {
                    if (userIsOnline)
                    {
                        u.DataUltimeiActivitati = DateTime.Now;
                        t.Update(u);
                        t.FlushSession();
                    }
                    return MapUser(u);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new MemberAccessException("Imposibil de recuperat datele utilizatorului", ex);
            }
        }

        public override MembershipUser GetUser(object providerUserKey, bool userIsOnline)
        {
            try
            {
                Utilizator u = UtilizatorRepository.Get(Convert.ToInt32(providerUserKey));
                if (u != null)
                {
                    if (userIsOnline)
                    {
                        u.DataUltimeiActivitati = DateTime.Now;
                        UtilizatorRepository.Update(u);
                        UtilizatorRepository.FlushSession();
                    }
                    return MapUser(u);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw new MemberAccessException("Imposibil de recuperat datele utilizatorului", ex);
            }
        }

        public override string GetUserNameByEmail(string email)
        {
            try
            {
                return UtilizatorRepository.GetUserNameByEmail(email, this.ApplicationName);
            }
            catch (Exception ex)
            {
                throw new MemberAccessException("Imposibil de recuperat datele utilizatorului", ex);
            }
        }

        public override string ResetPassword(string username, string answer)
        {
            throw new NotImplementedException();
        }

        public override bool UnlockUser(string userName)
        {
            try
            {
                var u = UtilizatorRepository.GetUserByName(userName, this.ApplicationName);
                if (u != null)
                {
                    u.Blocat = false;
                    UtilizatorRepository.Update(u);
                }
                return true;
            }
            catch (Exception ex)
            {
                throw new MemberAccessException("Imposibil de recuperat datele utilizatorului", ex);
            }
        }

        public override void UpdateUser(MembershipUser user)
        {
            try
            {
                var u = UtilizatorRepository.Get(Convert.ToInt32(user.ProviderUserKey));
                if (u != null)
                {
                    u.Email = user.Email;
                    u.Comentariu = user.Comment;
                    u.IntrebareParola = user.PasswordQuestion;
                    u.Aprobat = user.IsApproved;
                    UtilizatorRepository.Update(u);
                }
            }
            catch (Exception ex)
            {
                throw new MemberAccessException("Imposibil de recuperat datele utilizatorului", ex);
            }
        }

        public override bool ValidateUser(string username, string password)
        {
            var isValid = false;
            try
            {
                var repo = new UtilizatorRepository();
                var u = repo.GetUserByName(username, this.ApplicationName);
                if (u != null)
                {
                    if (CheckPassword(password, u.Parola))
                    {
                        if (u.Aprobat)
                        {
                            isValid = true;
                            u.DataAutentificariiAnterioare = u.DataUltimeiAutentificari;
                            u.DataUltimeiAutentificari = DateTime.Now;
                        }
                    }
                    else
                    {
                        u.NrAutentificariEsuate++;
                    }
                    repo.Update(u);
                }
            }
            catch (Exception ex)
            {
                throw new MemberAccessException("Imposibil de recuperat datele utilizatorului", ex);
            }
            return isValid;
        }

        #endregion
        #region Utility methods

        private void ValidateNewPasswordFormat(string username, string newPwd)
        {
            var args = new ValidatePasswordEventArgs(username, newPwd, false);
            args.FailureInformation = new MembershipPasswordException("Modificarea parolei s-a anulat datorita validarii esuate a parolei noi.");
            OnValidatingPassword(args);
            if (args.Cancel)
            {
                throw args.FailureInformation;
            }
        }

        private void ChangeUserPassword(string username, string newPwd)
        {
            var t = new Disertatie.Core.Repositories.UtilizatorRepository();
            var u = t.GetUserByName(username, this.ApplicationName);
            // u.Password = EncodePassword(newPwd);
            u.Parola = EncodePassword(newPwd);
            t.Update(u);
            t.FlushSession();
        }

        private void UpdateUserQuestionAndAnswer(string username, string newPwdQuestion, string newPwdAnswer)
        {
            var u = UtilizatorRepository.GetUserByName(username, this.ApplicationName);
            u.IntrebareParola = EncodePassword(newPwdQuestion);
            u.RaspunsParola = EncodePassword(newPwdAnswer);
            UtilizatorRepository.Update(u);
            UtilizatorRepository.FlushSession();
        }

        private bool DuplicatesEmailAddress(string email)
        {
            var dupEmail = false;
            if (RequiresUniqueEmail)
            {
                if (!string.IsNullOrEmpty(GetUserNameByEmail(email)))
                {
                    dupEmail = true;
                }
            }
            return dupEmail;
        }

        private void DeleteUserDataByName(string username)
        {
            Utilizator u = UtilizatorRepository.GetUserByName(username, this.ApplicationName);
            if (u != null)
            {
                UtilizatorRepository.Delete(u);
                UtilizatorRepository.FlushSession();
            }
        }

        private string RetrievePasswordFromDatabase(string username, string answerGiven)
        {
            Utilizator utilizator;
            string parola;
            string parolaRaspuns;
            try
            {
                utilizator = UtilizatorRepository.GetUserByName(username, this.ApplicationName);
                parola = utilizator.Parola;
                parolaRaspuns = utilizator.RaspunsParola;
            }
            catch (Exception ex)
            {
                throw new MemberAccessException("Eroare la procesarea datelor de autentificare", ex);
            }

            if (BadPasswordAnswerProvided(answerGiven, answerGiven))
            {
                utilizator.NrRaspunsuriEsuate++;
                UtilizatorRepository.Update(utilizator);
                throw new MembershipPasswordException("Raspuns parola gresit");
            }
            if (PasswordFormat == MembershipPasswordFormat.Encrypted)
            {
                parola = UnEncodePassword(parola);
            }
            return parola;
        }

        private bool BadPasswordAnswerProvided(string answer, string passwordAnswer)
        {
            if (RequiresQuestionAndAnswer)
            {
                return !CheckPassword(answer, passwordAnswer);
            }
            else return false;
        }

        private MembershipUserCollection MapUsers(IEnumerable<Utilizator> users)
        {
            var mapped_ = from u in users
                          select MapUser(u);

            var result = new MembershipUserCollection();
            foreach (var u in mapped_)
                result.Add(u);

            return result;
        }

        private MembershipUser MapUser(Utilizator u)
        {
            return new MembershipUser(
                            providerName: this.Name,
                            name: u.Username,
                            providerUserKey: u.Id,
                            email: u.Email,
                            passwordQuestion: u.IntrebareParola,
                            comment: u.Comentariu,
                            isApproved: u.Aprobat,
                            isLockedOut: u.Blocat,
                            creationDate: u.DataCreare,
                            lastLoginDate: u.DataUltimeiAutentificari.GetValueOrDefault(),
                            lastActivityDate: u.DataUltimeiActivitati.GetValueOrDefault(),
                            lastPasswordChangedDate: u.DataUltimeiModificariParola.GetValueOrDefault(),
                            lastLockoutDate: u.DataUltimeiBlocari.GetValueOrDefault());
        }

        private byte[] HexToByte(string hexString)
        {
            byte[] returnBytes = new byte[hexString.Length / 2];
            for (int i = 0; i < returnBytes.Length; i++)
                returnBytes[i] = Convert.ToByte(hexString.Substring(i * 2, 2), 16);
            return returnBytes;
        }

        private bool CheckPassword(string password, string dbpassword)
        {
            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Encrypted:
                    return password == UnEncodePassword(dbpassword);
                case MembershipPasswordFormat.Hashed:
                    return dbpassword == EncodePassword(password);
                default:
                    return password == dbpassword;
            }
        }

        private string EncodePassword(string password)
        {
            string encodedPassword = password;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    encodedPassword =
                        Convert.ToBase64String(EncryptPassword(Encoding.Unicode.GetBytes(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    var hash = new SHA1CryptoServiceProvider();
                    encodedPassword =
                        Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));
                    break;
                default:
                    throw new ProviderException("Format parola nesuportat");
            }

            return encodedPassword;
        }

        private string UnEncodePassword(string encodedPassword)
        {
            string password = encodedPassword;

            switch (PasswordFormat)
            {
                case MembershipPasswordFormat.Clear:
                    break;
                case MembershipPasswordFormat.Encrypted:
                    password =
                        Encoding.Unicode.GetString(DecryptPassword(Convert.FromBase64String(password)));
                    break;
                case MembershipPasswordFormat.Hashed:
                    throw new ProviderException("Nu se poate decoda parola");
                default:
                    throw new ProviderException("Format parola nesuportat");
            }

            return password;
        }

        #endregion
    }

}
