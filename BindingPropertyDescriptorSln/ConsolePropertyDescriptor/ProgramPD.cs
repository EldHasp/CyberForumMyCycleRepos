using System;

namespace ConsolePropertyDescriptor
{
	class ProgramPD
	{
		static void Main(string[] args)
		{
			// Создание источника данных (ViewModel)
			SourceClassPD source = new SourceClassPD();

			// Создание двух Представлений для одного источника
			TargetClassPD target1 = new TargetClassPD("Первый", source, nameof(source.Number));
			TargetClassPD target2 = new TargetClassPD("Второй", source, nameof(source.Number));

			// Цикл для изменения Представления и Источника
			do
			{
				Console.WriteLine();

				// Ввод данных в привязку
				Console.Write("Введите новое значение для target1.Value: ");
				target1.Value = Console.ReadLine();

				// Ввод данных в источник
				Console.Write("Введите новое значение для source.Number (только ЦЕЛОЕ ЧИСЛО!): ");
				source.Number = int.Parse(Console.ReadLine());

				Console.Write("Нажмите Enter для Продолжения или Esc для Выхода.....");
			} while (Console.ReadKey().Key != ConsoleKey.Escape);

		}
	}

}
