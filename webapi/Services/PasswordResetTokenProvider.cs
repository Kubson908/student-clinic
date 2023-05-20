using Microsoft.AspNetCore.DataProtection;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;

namespace Przychodnia.Webapi.Services
{
    public class PasswordResetTokenProvider<TUser> : DataProtectorTokenProvider<TUser> where TUser : class
    {
        public PasswordResetTokenProvider(IDataProtectionProvider dataProtectionProvider,
            IOptions<PasswordResetTokenProviderOptions> options,
            ILogger<DataProtectorTokenProvider<TUser>> logger)
            : base(dataProtectionProvider, options, logger)
        {
            
        }

        public class PasswordResetTokenProviderOptions : DataProtectionTokenProviderOptions
        {
            public PasswordResetTokenProviderOptions()
            {
                Name = "PasswordResetTokenProvider";
                TokenLifespan = TimeSpan.FromMinutes(15);
            }
        }
    }
}
