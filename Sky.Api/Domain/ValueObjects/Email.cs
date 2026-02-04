using System.Text.RegularExpressions;

namespace Sky.Api.Domain.ValueObjects
{
    public sealed partial record Email
    {
        #region Constants

        public const int MaxLength = 160;
        public const int MinLength = 6;
        public const string Pattern = @"^\w+([-+.']\w+)*@\w+([-.]\w+)*\.\w+([-.]\w+)*$";

        #endregion

        #region Properties
        public string Address { get; private set; } = string.Empty;
        #endregion

        #region Constructors

        private Email()
        {

        }

        private Email(string address)
        {
            Address = address;
        }
        #endregion

        #region Factories

        public static Email Create(string address)
        {
            if (string.IsNullOrEmpty(address) || string.IsNullOrWhiteSpace(address))
                throw new ArgumentException("O email não pode ser nulo.");


            address = address.Trim();
            address = address.ToLower();

            if (!EmailRegex().IsMatch(address))
                throw new ArgumentException("Email inválido");

            return new Email(address);

        }

        #endregion

        #region Operators

        public static implicit operator string(Email email) => email.ToString();


        #endregion

        #region Overrides

        public override string ToString() => Address;

        #endregion

        #region Others

        [GeneratedRegex(Pattern)]
        private static partial Regex EmailRegex();

        #endregion
    }
}
