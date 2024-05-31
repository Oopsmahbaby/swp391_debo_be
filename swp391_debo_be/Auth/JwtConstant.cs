﻿namespace swp391_debo_be.Auth
{
    public class JwtConstant
    {
        /// <summary>
        /// Provider constans of the header.
        /// </summary>
        public class Header
        {
            /// <summary>
            /// The key authorization of the header.
            /// </summary>
            public const string Authorization = nameof(Authorization);

            /// <summary>
            /// The postfix of the authorization value.
            /// </summary>
            public const string Bearer = nameof(Bearer);
        }

        /// <summary>
        /// Provider constans of the key claim.
        /// </summary>
        public class KeyClaim
        {
            public const string Username = nameof(Username);
            public const string Email = nameof(Email);
            public const string Password = nameof(Password);
        }
    }
}

