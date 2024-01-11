internal static class Constants
{
    public const int MaxImageWidth = 250;
    public const int MaxImageHeight = 250;
    public const int MaxImageLogoWidth = 400;
    public const int MaxImageLogoHeight = 150;
    public const string SearchBoxData = "search";
    public const string DateFormat = "dd/MM/yyyy";
    public const string WhatsAppURL = "https://api.whatsapp.com/send?phone=";
    public const string ThrowException = "Exception";
    public const string ThrowDBException = "DBException";
    public const string CustomErrorView = "CustomError";
    public const string PasswordValidationRegex =
        @"^(?=.*[A-Z])(?=.*[a-z])(?=.*\d)(?=.*[@#$!%*?&;]).{8,}$";
    public const string DefaultSystemLanguage = "pt_BR";
    public const int QtRegistersPagination = 5;
    public const int InitialPageForPagination = 1;
}
