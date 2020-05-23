using System;

namespace ConsolePropertyDescriptorSource
{
	class ProgramPDS
	{
		static void Main(string[] args)
		{
			// Создание источника данных (ViewModel)
			SourceClassPDS source = new SourceClassPDS();

			// Создание двух Представлений для одного источника
			TargetClassPD target1 = new TargetClassPD("Первый", source, nameof(source.Text));
			TargetClassPD target2 = new TargetClassPD("Второй", source, nameof(source.Text));
			TargetClassPD target3 = new TargetClassPD("Третий", source, nameof(source.Length));

			// Цикл для изменения Представления и Источника
			do
			{
				Console.WriteLine();

				// Ввод данных в привязку
				Console.Write("Введите новое значение для target1.Value: ");
				target1.Value = Console.ReadLine();

				// Ввод данных в источник
				Console.Write("Введите новое значение для source.Text: ");
				source.Text = Console.ReadLine();

				Console.Write("Нажмите Enter для Продолжения или Esc для Выхода.....");
			} while (Console.ReadKey().Key != ConsoleKey.Escape);
		}
	}
}
