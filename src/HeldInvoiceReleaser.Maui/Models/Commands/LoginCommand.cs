using FluentValidation;

namespace HeldInvoiceReleaser.Maui.Models.Commands
{
    public class LoginCommand
    {
        public string ServerAddress { get; set; }
        public string LocationId { get; set; }
    }

    public class LoginCommandValidator : AbstractValidator<LoginCommand>
    {
        public LoginCommandValidator()
        {
            RuleFor(x => x.ServerAddress)
                .NotEmpty();

            RuleFor(x => x.ServerAddress)
                .NotEmpty()
                .Must(ServerMustBeAUri)
                .WithMessage("Server Address must be a valid http URI. eg: http://www.SomeWebSite.com.au");
        }

        private static bool ServerMustBeAUri(string link)
        {
            if (string.IsNullOrWhiteSpace(link))
            {
                return false;
            }

            Uri outUri;
            var isUri =  Uri.TryCreate(link, UriKind.Absolute, out outUri);
            return isUri && (outUri.Scheme == Uri.UriSchemeHttp || outUri.Scheme == Uri.UriSchemeHttps);
        }
    }
}
