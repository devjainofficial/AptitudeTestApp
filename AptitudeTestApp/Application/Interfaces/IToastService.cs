namespace AptitudeTestApp.Application.Interfaces;

public interface IToastService
{
    void Info(string summary, string detail, int duration);
    void Success(string summary, string detail, int duration);
    void Warning(string summary, string detail, int duration);
    void Error(string summary, string detail, int duration);
}
