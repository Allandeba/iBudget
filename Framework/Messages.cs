public class Messages
{
    public const string EmptyTextValidation = "{0} não pode ser vazio";
    public const string InvalidFormatValidation = "Formato inválido {0}";
    public const string MaxLengthValidation = "{0} precisa ter menos de {1} caracteres";
    public const string RangeValidation = "{0} precisa ser entre {1} e {2}";
    public const string MinValueValidation = "{0} precisa ser mais que {1}";
    public const string WhatsAppMessage =
        "Olá, essa é uma mensagem automática de {0}.\nVocê pode fazer o download do seu orçamento através desse link a seguir:\n{1}";
    public const string PasswordValidation =
        "A senha deve conter pelo menos uma letra maiúscula, uma letra minúscula, um número e um caractere especial. Ela também deve ter pelo menos 8 caracteres.";
    public const string UniqueDBMessage = "Já existe um registro com esse valor";
    public const string ForeignKeyDBMessage =
        "Esse registro já esta em uso e não pode ser excluído";
    public const string GenericDBExceptionMessage =
        "Ocorreu um erro no banco de dados, por favor entre em contato com o administrador e informe a rotina que causou a inconsistência";
    public const string CompanyNotFoundMessage = "Você precisa configurar uma empresa antes";
}
