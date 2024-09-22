namespace InterserviceCommunication.Models
{
    /// <summary>
    /// Модель для сериализации, описывающая результат выполнения некой операции в формате строки
    /// </summary>
    public class StringResponseModel
    {
        /// <summary>
        /// Результат операции
        /// </summary>
        public string Result { get; set; } = string.Empty;
    }
}
