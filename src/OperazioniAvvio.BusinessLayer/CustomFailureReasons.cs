namespace OperazioniAvvio.BusinessLayer;

public static class CustomFailureReasons
{
	public const int NotAvailable = FailureReasons.GenericError + 1; // int = 1001
	public const int InvalidRequest = FailureReasons.GenericError + 2; // int = 1002
}