namespace InterserviceCommunication.Models
{
	/// <summary>
	/// Модель для сериализации, описывающая результат выполнения некой операции в виде перечислимого списка
	/// </summary>
	/// <typeparam name="TResult">Тип элемента списка результата выполнения операции</typeparam>
	public class EnumerableResponseModel<TResult>
	{
		/// <summary>
		/// Результат операции
		/// </summary>
		public IEnumerable<TResult> Result { get; set; } = null!;
	}
}
