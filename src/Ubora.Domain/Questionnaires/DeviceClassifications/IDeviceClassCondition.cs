namespace Ubora.Domain.Questionnaires.DeviceClassifications
{
    public interface IDeviceClassCondition
    {
        void Validate(DeviceClassificationQuestionnaireTree questionnaireTree);
        bool IsSatisfied(DeviceClassificationQuestionnaireTree questionnaireTree);
    }
}