#pragma once
#include "../Engine/src/Engine.h"
#include "../Engine/src/Engine/Core/EntryPoint.h"

#include "ManagedObject.h"

namespace Wrapper
{
	public ref struct ImGUI
	{
		static void Begin(String^ name, bool% p_open) {Engine::ImGUI::Begin(stringsToCStrings(name), CLIPointerToNativePointer<bool>(p_open)); }
		static void Begin(String^ name) { Engine::ImGUI::Begin(stringsToCStrings(name)); }
		static void ColorEdit3(String^ label, float col[3]) { Engine::ImGUI::ColorEdit3(stringsToCStrings(label), col); }
		static void End() { Engine::ImGUI::End(); }
		static void SliderInt(String^ label, int% v, int v_min, int v_max) { Engine::ImGUI::SliderInt(stringsToCStrings(label), CLIPointerToNativePointer<int>(v), v_min, v_max); }
		static void SliderInt2(String^ label, array<System::Int32>^ v, int v_min, int v_max) { Engine::ImGUI::SliderInt2(stringsToCStrings(label), intArrayToPointer(v), v_min, v_max); }
		static void SliderInt3(String^ label, array<System::Int32>^ v, int v_min, int v_max) { Engine::ImGUI::SliderInt3(stringsToCStrings(label), intArrayToPointer(v), v_min, v_max); }
		static void SliderInt4(String^ label, array<System::Int32>^ v, int v_min, int v_max) { Engine::ImGUI::SliderInt4(stringsToCStrings(label), intArrayToPointer(v), v_min, v_max); }
		static void Checkbox(String^ label, bool% v) { Engine::ImGUI::Checkbox(stringsToCStrings(label), CLIPointerToNativePointer<bool>(v)); }
		static void SliderFloat(String^ label, float% v, float v_min, float v_max) { Engine::ImGUI::SliderFloat(stringsToCStrings(label), CLIPointerToNativePointer<float>(v), v_min, v_max); }
		static void SliderFloat2(String^ label, array<System::Single>^ v, float v_min, float v_max) { Engine::ImGUI::SliderFloat2(stringsToCStrings(label), floatArrayToPointer(v), v_min, v_max); }
		static void SliderFloat3(String^ label, array<System::Single>^ v, float v_min, float v_max) { Engine::ImGUI::SliderFloat3(stringsToCStrings(label), floatArrayToPointer(v), v_min, v_max); }
		static void SliderFloat4(String^ label, array<System::Single>^ v, float v_min, float v_max) { Engine::ImGUI::SliderFloat4(stringsToCStrings(label), floatArrayToPointer(v), v_min, v_max); }
		static void ColorEdit4(String^ label, array<System::Single>^ col) { Engine::ImGUI::ColorEdit4(stringsToCStrings(label), floatArrayToPointer(col)); }
		static void ColorPicker3(String^ label, array<System::Single>^ col) { Engine::ImGUI::ColorPicker3(stringsToCStrings(label), floatArrayToPointer(col)); }
		static void ColorPicker4(String^ label, array<System::Single>^ col) { Engine::ImGUI::ColorPicker4(stringsToCStrings(label), floatArrayToPointer(col)); }
		static void DragFloat(String^ label, float% v, float v_speed, float v_min, float v_max) { Engine::ImGUI::DragFloat(stringsToCStrings(label), CLIPointerToNativePointer<float>(v), v_speed, v_min, v_max); }
		static void DragFloat2(String^ label, array<System::Single>^ v, float v_speed, float v_min, float v_max) { Engine::ImGUI::DragFloat2(stringsToCStrings(label), floatArrayToPointer(v), v_speed, v_min, v_max); }
		static void DragFloat3(String^ label, array<System::Single>^ v, float v_speed, float v_min, float v_max) { Engine::ImGUI::DragFloat3(stringsToCStrings(label), floatArrayToPointer(v), v_speed, v_min, v_max); }
		static void DragFloat4(String^ label, array<System::Single>^ v, float v_speed, float v_min, float v_max) { Engine::ImGUI::DragFloat4(stringsToCStrings(label), floatArrayToPointer(v), v_speed, v_min, v_max); }
		static void DragInt(String^ label, int% v, float v_speed, int v_min, int v_max) { Engine::ImGUI::DragInt(stringsToCStrings(label), CLIPointerToNativePointer<int>(v), v_speed, v_min, v_max); };
		static void DragInt2(String^ label, array<System::Int32>^ v, float v_speed, int v_min, int v_max) { Engine::ImGUI::DragInt2(stringsToCStrings(label), intArrayToPointer(v), v_speed, v_min, v_max); }
		static void DragInt3(String^ label, array<System::Int32>^ v, float v_speed, int v_min, int v_max) { Engine::ImGUI::DragInt3(stringsToCStrings(label), intArrayToPointer(v), v_speed, v_min, v_max); }
		static void DragInt4(String^ label, array<System::Int32>^ v, float v_speed, int v_min, int v_max) { Engine::ImGUI::DragInt4(stringsToCStrings(label), intArrayToPointer(v), v_speed, v_min, v_max); }
		static void SliderAngle(String^ label, float% v_rad, float v_degrees_min, float v_degrees_max) { Engine::ImGUI::SliderAngle(stringsToCStrings(label), CLIPointerToNativePointer<float>(v_rad), v_degrees_min, v_degrees_max); }
	};

	public ref struct Input
	{
		static bool IsKeyPressed(int keycode) { return Engine::Input::IsKeyPressed(keycode); }
		static bool IsMouseButtonPressed(int button) { return Engine::Input::IsMouseButtonPressed(button); }
		static float GetMouseX() { return Engine::Input::GetMouseX(); }
		static float GetMouseY() { return Engine::Input::GetMouseY(); }
		static Tuple<float, float>^ GetMousePosition() { auto var = Engine::Input::GetMousePosition(); return gcnew Tuple<float, float>(var.first, var.second); }
	};

	public enum class Key
	{
		Space = 32,
		Apostrophe = 39,
		Comma = 44,
		Minus = 45,
		Period = 46,
		Slash = 47,
		Zero = 48,
		One = 49,
		Two = 50,
		Three = 51,
		Four = 52,
		Five = 53,
		Six = 54,
		Seven = 55,
		Eight = 56,
		Nine = 57,
		Semicolon = 59,
		Equal = 61,
		A = 65,
		B = 66,
		C = 67,
		D = 68,
		E = 69,
		F = 70,
		G = 71,
		H = 72,
		I = 73,
		J = 74,
		K = 75,
		L = 76,
		M = 77,
		N = 78,
		O = 79,
		P = 80,
		Q = 81,
		R = 82,
		S = 83,
		T = 84,
		U = 85,
		V = 86,
		W = 87,
		X = 88,
		Y = 89,
		Z = 90,
		Left_bracket = 91,
		Backslash = 92,
		Right_bracket = 93,
		Grave_accent = 96,
		World_1 = 161,
		World_2 = 162,
		Escape = 256,
		Enter = 257,
		Tab = 258,
		Backspace = 259,
		Insert = 260,
		Delete = 261,
		Right = 262,
		Left = 263,
		Down = 264,
		Up = 265,
		Page_up = 266,
		Page_down = 267,
		Home = 268,
		End = 269,
		Caps_lock = 280,
		Scroll_lock = 281,
		Num_lock = 282,
		Print_screen = 283,
		Pause = 284,
		F1 = 290,
		F2 = 291,
		F3 = 292,
		F4 = 293,
		F5 = 294,
		F6 = 295,
		F7 = 296,
		F8 = 297,
		F9 = 298,
		F10 = 299,
		F11 = 300,
		F12 = 301,
		F13 = 302,
		F14 = 303,
		F15 = 304,
		F16 = 305,
		F17 = 306,
		F18 = 307,
		F19 = 308,
		F20 = 309,
		F21 = 310,
		F22 = 311,
		F23 = 312,
		F24 = 313,
		F25 = 314,
		KP_0 = 320,
		KP_1 = 321,
		KP_2 = 322,
		KP_3 = 323,
		KP_4 = 324,
		KP_5 = 325,
		KP_6 = 326,
		KP_7 = 327,
		KP_8 = 328,
		KP_9 = 329,
		KP_decimal = 330,
		KP_divide = 331,
		KP_multiply = 332,
		KP_subtract = 333,
		KP_add = 334,
		KP_enter = 335,
		KP_equal = 336,
		Left_shift = 340,
		Left_control = 341,
		Left_aly = 342,
		Left_super = 343,
		Right_shift = 344,
		Right_control = 345,
		Right_alt = 346,
		Right_windows = 347,
		Menu = 348
	};

	public ref struct KeyTo
	{
		static char String(int value)
		{
			switch (value)
			{
			case (int)Key::Space: return ' ';
			case (int)Key::Apostrophe: return '\'';
			case (int)Key::Comma: return ',';
			case (int)Key::Minus: return '-';
			case (int)Key::Period: return '.';
			case (int)Key::Slash: return '/';
			case (int)Key::Zero: return '0';
			case (int)Key::One: return '1';
			case (int)Key::Two: return '2';
			case (int)Key::Three: return '3';
			case (int)Key::Four: return '4';
			case (int)Key::Five: return '5';
			case (int)Key::Six: return '6';
			case (int)Key::Seven: return '7';
			case (int)Key::Eight: return '8';
			case (int)Key::Nine: return '9';
			case (int)Key::Semicolon: return ';';
			case (int)Key::Equal: return '=';
			case (int)Key::A: return 'a';
			case (int)Key::B: return 'b';
			case (int)Key::C: return 'c';
			case (int)Key::D: return 'd';
			case (int)Key::E: return 'e';
			case (int)Key::F: return 'f';
			case (int)Key::G: return 'g';
			case (int)Key::H: return 'h';
			case (int)Key::I: return 'i';
			case (int)Key::J: return 'j';
			case (int)Key::K: return 'k';
			case (int)Key::L: return 'l';
			case (int)Key::M: return 'm';
			case (int)Key::N: return 'n';
			case (int)Key::O: return 'o';
			case (int)Key::P: return 'p';
			case (int)Key::Q: return 'q';
			case (int)Key::R: return 'r';
			case (int)Key::S: return 's';
			case (int)Key::T: return 't';
			case (int)Key::U: return 'u';
			case (int)Key::V: return 'v';
			case (int)Key::W: return 'w';
			case (int)Key::X: return 'x';
			case (int)Key::Y: return 'y';
			case (int)Key::Z: return 'z';
			case (int)Key::Enter: return '\n';
			case (int)Key::Left_bracket: return '[';
			case (int)Key::Backslash: return '\\';
			case (int)Key::Right_bracket: return ']';
			case (int)Key::Grave_accent: return '`';
			case (int)Key::KP_decimal: return '.';
			case (int)Key::KP_divide: return '/';
			case (int)Key::KP_multiply: return '*';
			case (int)Key::KP_subtract: return '-';
			case (int)Key::KP_add: return '+';
			case (int)Key::KP_equal: return '=';
			case (int)Key::KP_enter: return '\n';
			default: return '\0';
			}
		}

		static int Int(int value)
		{
			switch (value)
			{
			case (int)Key::Zero: return 0;
			case (int)Key::One: return 1;
			case (int)Key::Two: return 2;
			case (int)Key::Three: return 3;
			case (int)Key::Four: return 4;
			case (int)Key::Five: return 5;
			case (int)Key::Six: return 6;
			case (int)Key::Seven: return 7;
			case (int)Key::Eight: return 8;
			case (int)Key::Nine: return 9;
			default: return -1;
			}
			
		}

		static char String(Key value)
		{
			switch (value)
			{
			case Key::Space: return ' ';
			case Key::Apostrophe: return '\'';
			case Key::Comma: return ',';
			case Key::Minus: return '-';
			case Key::Period: return '.';
			case Key::Slash: return '/';
			case Key::Zero: return '0';
			case Key::One: return '1';
			case Key::Two: return '2';
			case Key::Three: return '3';
			case Key::Four: return '4';
			case Key::Five: return '5';
			case Key::Six: return '6';
			case Key::Seven: return '7';
			case Key::Eight: return '8';
			case Key::Nine: return '9';
			case Key::Semicolon: return ';';
			case Key::Equal: return '=';
			case Key::A: return 'a';
			case Key::B: return 'b';
			case Key::C: return 'c';
			case Key::D: return 'd';
			case Key::E: return 'e';
			case Key::F: return 'f';
			case Key::G: return 'g';
			case Key::H: return 'h';
			case Key::I: return 'i';
			case Key::J: return 'j';
			case Key::K: return 'k';
			case Key::L: return 'l';
			case Key::M: return 'm';
			case Key::N: return 'n';
			case Key::O: return 'o';
			case Key::P: return 'p';
			case Key::Q: return 'q';
			case Key::R: return 'r';
			case Key::S: return 's';
			case Key::T: return 't';
			case Key::U: return 'u';
			case Key::V: return 'v';
			case Key::W: return 'w';
			case Key::X: return 'x';
			case Key::Y: return 'y';
			case Key::Z: return 'z';
			case Key::Enter: return '\n';
			case Key::Left_bracket: return '[';
			case Key::Backslash: return '\\';
			case Key::Right_bracket: return ']';
			case Key::Grave_accent: return '`';
			case Key::KP_decimal: return '.';
			case Key::KP_divide: return '/';
			case Key::KP_multiply: return '*';
			case Key::KP_subtract: return '-';
			case Key::KP_add: return '+';
			case Key::KP_equal: return '=';
			case Key::KP_enter: return '\n';
			default: return '\0';
			}
		}

		static int Int(Key value)
		{
			switch (value)
			{
			case Key::Zero: return 0;
			case Key::One: return 1;
			case Key::Two: return 2;
			case Key::Three: return 3;
			case Key::Four: return 4;
			case Key::Five: return 5;
			case Key::Six: return 6;
			case Key::Seven: return 7;
			case Key::Eight: return 8;
			case Key::Nine: return 9;
			default: return -1;
			}

		}
	};

	public enum class Mouse
	{
		Mouse_button_1      =    0						,
		Mouse_button_2      =    1						,
		Mouse_button_3      =    2						,
		Mouse_button_4      =    3						,
		Mouse_button_5      =    4						,
		Mouse_button_6      =    5						,
		Mouse_button_7      =    6						,
		Mouse_button_8      =    7						,
		Mouse_button_last   =    Mouse_button_8			,
		Mouse_button_left   =    Mouse_button_1			,
		Mouse_button_right  =    Mouse_button_2			,
		Mouse_button_middle =    Mouse_button_3			,
		mod_shift           =    0x0001					,
		mod_control         =    0x0002					,
		mod_alt             =    0x0004					,
		mod_super           =    0x0008					,
		mod_caps_lock       =    0x0010					,
		mod_num_lock        =    0x0020					,
	};
}