/**********************************************************************
 * The MIT License (MIT)
 *
 * Copyright (c) 2014, XLazz, Inc.
 * Contacts: http://xlazz.com
 *
 * Permission is hereby granted, free of charge, to any person obtaining a copy
 * of this software and associated documentation files (the "Software"), to deal
 * in the Software without restriction, including without limitation the rights
 * to use, copy, modify, merge, publish, distribute, sublicense, and/or sell
 * copies of the Software, and to permit persons to whom the Software is
 * furnished to do so, subject to the following conditions:
 *
 * The above copyright notice and this permission notice shall be included in
 * all copies or substantial portions of the Software.
 *
 * THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
 * IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
 * FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT. IN NO EVENT SHALL THE
 * AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
 * LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
 * OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
 * THE SOFTWARE.
 **********************************************************************/

using UnityEngine;
using System.Collections;

public class Lang 
{
	public static string MainMenu_Start;
	public static string MainMenu_Settings;
	public static string MainMenu_Help;
	public static string MainMenu_Credits;
	public static string MainMenu_Exit;
	public static string MainMenu_Hint;

	public static string Settings_LanguageName;
	public static string Settings_Language;
	public static string Settings_Stereo;
	public static string Settings_StartWith;
	public static string Settings_ConfigurationFile;
	public static string Settings_Debug;	
	public static string Settings_Off;	
	public static string Settings_On;	
	public static string Settings_MainMenu;	
	public static string Settings_AR;	
	public static string Settings_Back;	

	public static string About_Text;	
	public static string Abot_Caption;		

	public static string Help_Caption;		
	public static string Help_Text;
	
	public static string Details_Description;
	public static string Details_3DModel;
	public static string Details_Videos;
	public static string Details_Images;

	public enum LangType 
	{
		Russia,
		English
	}

	private static LangType _type;
	public static LangType Type { get { return _type; } }

	static Lang()
	{
		try
		{
			var val = (Lang.LangType) System.Enum.Parse(typeof (Lang.LangType), PlayerPrefs.GetString("LangType"));
			_type = val;
		} catch {
			_type = LangType.English;			
		}

		SwitchTo(_type);
	}

	public static void SwitchTo(Lang.LangType type)
	{
		_type = type;
		if (_type == LangType.English) InitEnglish();
		if (_type == LangType.Russia) InitRissian();
		PlayerPrefs.SetString("LangType", _type.ToString());
	}

	public static void InitEnglish()
	{
		MainMenu_Start = "Start";
		MainMenu_Settings = "Settings";
		MainMenu_Help = "Help";
		MainMenu_Credits = "Credits";
		MainMenu_Exit = "Exit";
		MainMenu_Hint = "Use touchpad to select menu items";

		Settings_Language = "Language";
		Settings_Stereo = "Stereo";
		Settings_StartWith = "Start with";
		Settings_ConfigurationFile = "Config file";
		Settings_Debug = "Debug";	
		Settings_LanguageName = "English";
		Settings_Off = "Off";
		Settings_On = "On";
		Settings_MainMenu = "Main menu";
		Settings_AR = "Markers detection";
		Settings_Back = "Back";

		About_Text = "Pavel Ryabenko:\nLead programmer,\nAugmented Reality and Computer Vision Expert\n\nYana Artishcheva:\nProject Manager, Programmer,\nWearable Solutions Development Expert\n\nContacts: yanaa@xlazz.com\nXLazz, Inc. (C) 2014, http://xlazz.com";
		Abot_Caption = "Credits";

		Help_Caption = "Help";
		Help_Text = "In main menu press \"Start\" to enter markers recognition mode.\nWhen Moverio camera detects the marker in marker recognition mode the button with brief marker info will appears on the screen. Taping this button you'll see menu that permits you get detailed info about item this marker assigned to. To close detailed information menu click on cross in upper right menu corner or press hardware back button on CPU Unit.\nPress screen button \"Back\" or hardware back button on CPU Unit to return in main menu from markers recognition screen.\n\nIn \"Settings\" you will find the following options:\n\n\"Language\" -  switches the application interface language between English/Russian (it does not affects to the content language that depends on external files).\n\n\"Stereo\" - permits turn application in stereroscopic/2D modes. That affects only on markers recognition mode screen. Other application screens are in the 2D.\n\n\"Start with\" - set what screen user will see when application starts - main menu or marker recognition screen.\n\n\"Config file\" - permits to choose XML file with application content description and references.\n\n\"Debug\" - turns on/off debug mode. When debug is on user see camera image in markers recognition screen.";

		Details_Description = "Description";
		Details_3DModel = "3d Model";
		Details_Images = "Images";
		Details_Videos = "Promotional video";
	}

	public static void InitRissian()
	{
		MainMenu_Start = "Старт";
		MainMenu_Settings = "Настройки";
		MainMenu_Help = "Справка";
		MainMenu_Credits = "О разработчиках";
		MainMenu_Exit = "Выход";
		MainMenu_Hint = "Используйте touchpad для выбора пунктов меню";

		Settings_Language = "Язык";
		Settings_Stereo = "Стерео";
		Settings_StartWith = "Начинать с";
		Settings_ConfigurationFile = "Файл настроек";
		Settings_Debug = "Отладка";	
		Settings_LanguageName = "Русский";		
		Settings_Off = "выкл";
		Settings_On = "вкл";
		Settings_MainMenu = "Главного меню";
		Settings_AR = "Распознавания";
		Settings_Back = "Назад";

		About_Text = "Павел Рябенко:\nВедущий программист,\nЭксперт по компьютерному зрению и дополненной реальности\n\nЯна Артищева:\nКоординатор проекта, программист,\nЭксперт по разработке носимых систем\n\nКонтакты: yanaa@xlazz.com\nXLazz, Inc. (C) 2014, http://xlazz.com";
		Abot_Caption = "О разработчиках";

		Help_Caption = "Справка";
		Help_Text = "Выбрав в главном меню пункт \"Старт\", Вы переключите приложение в режим распознавания маркеров.\n\nКогда в режиме распознавания маркеров камера очков Moverio распознаёт маркер, на экране появляется кнопка с краткой информацией о маркере. Нажав на эту кнопку, вы откроете меню, позволяющее получить более полную информацию об объекте, соответствующем маркеру. Чтобы закрыть это меню, нажмите на крестик в его правом верхнем углу или нажмите кнопку \"Back\" на системном блоке очков.\n\nНажмите экранную кнопку \"Назад\" или аппаратную кнопку \"Back\" на системном блоке, чтобы выйти из режима распознавания маркеров в главное меню.\n\nВ меню \"Настройки\" Вы можете настроить внешний вид и поведение приложения.\n\n\"Язык\" - позволяет выбрать язык интерфейса приложения (русский или английский). Это не повлияет на язык, использующийся при описании маркированных объектов, т.к. эти тексты хранятся во внешних файлах.\n\n\"Стерео\" - устанавливает стереоскопический или 2D видео-режим для экрана распознавания маркеров. Остальные экраны приложения работают только в 2D-режиме.\n\n\"Начинать с\" - позволяет выбрать, что произойдёт после запуска приложения - появится главное меню или сразу запустится режим распознавания маркеров.\n\n\"Файл настроек\" - позволяет выбрать XML-файл с описанием контента и ссылками на графику и видео.\n\n\"Отладка\" - включает/выключает режим отладки. При включённом режиме отладки на экран распознавания маркеров выводится картинка с камеры очков.";

		Details_Description = "Описание";
		Details_3DModel = "3D модель";
		Details_Images = "Презентационное видео";
		Details_Videos = "Фотографии";
	}

}