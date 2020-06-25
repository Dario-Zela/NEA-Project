#pragma once

namespace Engine
{
	class ENGINE_API ImGUI
	{
	public:
		static void Begin(const char* name, bool* p_open = false); 
		static void ColorEdit3(const char* label, float col[3]); 
		static void End();
		static void SliderInt(const char* label, int* v, int v_min, int v_max);
		static void SliderInt2(const char* label, int v[2], int v_min, int v_max);
		static void SliderInt3(const char* label, int v[3], int v_min, int v_max);
		static void SliderInt4(const char* label, int v[4], int v_min, int v_max);
		static void Checkbox(const char* label, bool* v);
		static void SliderFloat(const char* label, float* v, float v_min, float v_max);
		static void SliderFloat2(const char* label, float v[2], float v_min, float v_max);
		static void SliderFloat3(const char* label, float v[3], float v_min, float v_max);
		static void SliderFloat4(const char* label, float v[4], float v_min, float v_max);
		static void ColorEdit4(const char* label, float col[4]);
		static void ColorPicker3(const char* label, float col[3]);
		static void ColorPicker4(const char* label, float col[4]);
		static void DragFloat(const char* label, float* v, float v_speed, float v_min, float v_max);
		static void DragFloat2(const char* label, float v[2], float v_speed, float v_min, float v_max);
		static void DragFloat3(const char* label, float v[3], float v_speed, float v_min, float v_max);
		static void DragFloat4(const char* label, float v[4], float v_speed, float v_min, float v_max);
		static void DragInt(const char* label, int* v, float v_speed, int v_min, int v_max);
		static void DragInt2(const char* label, int v[2], float v_speed, int v_min, int v_max);
		static void DragInt3(const char* label, int v[3], float v_speed, int v_min, int v_max);
		static void DragInt4(const char* label, int v[4], float v_speed, int v_min, int v_max);
		static void SliderAngle(const char* label, float* v_rad, float v_degrees_min, float v_degrees_max);
	};
}