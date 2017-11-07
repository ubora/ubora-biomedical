namespace Ubora.Domain.Questionnaires.DeviceClassifications
{
    public interface IDeviceClassCondition
    {
        bool IsFulfilled(DeviceClassificationQuestionnaireTree questionnaireTree);
    }
}