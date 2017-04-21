using System;
using Microsoft.Extensions.Configuration;

namespace Ubora.Web.Extensions
{
    public static class ConfigurationExtensions
    {
        public static bool SetEnvironmentVariableAwsCredentials(this IConfiguration configuration)
        {
            if ((configuration["AWS_ACCESS_KEY_ID"] == null || configuration["AWS_SECRET_ACCESS_KEY"] == null) &&
                (configuration["AWS_ACCESS_KEY_ID_FROM_USERSECRET"] == null ||
                 configuration["AWS_SECRET_ACCESS_KEY_FROM_USERSECRET"] == null))
            {
                return false;
            }

            if (configuration["AWS_ACCESS_KEY_ID"] == null)
            {
                SetEnvironmentVariableAccessKeyIdFromUserSecret(configuration);
            }

            if (configuration["AWS_SECRET_ACCESS_KEY"] == null)
            {
                SetEnvironmentVariableSecretAccessKeyFromUserSecret(configuration);
            }

            return true;
        }

        private static void SetEnvironmentVariableAccessKeyIdFromUserSecret(IConfiguration configuration)
        {
            Environment.SetEnvironmentVariable("AWS_ACCESS_KEY_ID",
                configuration["AWS_ACCESS_KEY_ID_FROM_USERSECRET"]);
        }

        private static void SetEnvironmentVariableSecretAccessKeyFromUserSecret(IConfiguration configuration)
        {
            Environment.SetEnvironmentVariable("AWS_SECRET_ACCESS_KEY",
                configuration["AWS_SECRET_ACCESS_KEY_FROM_USERSECRET"]);
        }
    }
}
