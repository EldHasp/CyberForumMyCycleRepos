using System;

namespace ConsoleINPC
{
	// Контролер
	class Program
	{
		static void Main(string[] args)
		{
			// Создание источника данных (ViewModel)
			SourceClass source = new SourceClass();

			// Создание двух Представлений для одного источника
			TargetClass<SourceClass> target1 = new TargetClass<SourceClass>("Первый", source, nameof(source.Number));
			TargetClass<SourceClass> target2 = new TargetClass<SourceClass>("Второй", source, nameof(source.Number));

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
