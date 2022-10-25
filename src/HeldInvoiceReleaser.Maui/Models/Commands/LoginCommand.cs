using FluentValidation;

namespace HeldInvoiceReleaser.Maui.Models.Commands
{
    public class LoginCommand
    {
        public string Server { get; set; }
        public string Location { get; set; }
    }

    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.Server)
                .Must(ServerMustBeAUri)
                .WithMessage("Server '{PropertyValue}' must be a valid URI. eg: http://www.SomeWebSite.com.au");
        }

        private static bool ServerMustBeAUri(string link)
        {
            if (string.IsNullOrWhiteSpace(link))
            {
                return false;
            }

            //Courtesy of @Pure.Krome's comment and https://stackoverflow.com/a/25654227/563532
            Uri outUri;
            return Uri.TryCreate(link, UriKind.Absolute, out outUri)
                   && (outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps);
        }
    }
}
