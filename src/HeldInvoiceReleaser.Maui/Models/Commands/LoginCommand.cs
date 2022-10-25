using FluentValidation;

namespace HeldInvoiceReleaser.Maui.Models.Commands
{
    public class LoginCommand
    {
        public string ServerAddress { get; set; }
        public string Location { get; set; }
    }

    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.ServerAddress)
                .Must(ServerMustBeAUri)
                .WithMessage("Server Address must be a valid http URI. eg: http://www.SomeWebSite.com.au");
        }

        private static bool ServerMustBeAUri(string link)
        {
            if (string.IsNullOrWhiteSpace(link))
            {
                return false;
            }

            return Uri.TryCreate(link, UriKind.Absolute, out var outUri)
                   && (outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps);
        }
    }
}
