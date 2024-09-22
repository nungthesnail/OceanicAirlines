namespace FrontendService.Models
{
    /// <summary>
    /// Модель отображения информации об ошибке из сгенерированного шаблона проекта MVC
    /// </summary>
    public class ErrorViewModel
    {
        /// <summary>
        /// Идентификатор запроса
        /// </summary>
        public string? RequestId { get; set; }

        /// <summary>
        /// Определяет, показывать ли идентификатор запроса
        /// </summary>
        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }
}
