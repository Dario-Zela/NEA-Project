#include "ENPH.h"
#include "ImGUIWrapper.h"
#include "imgui.h"

namespace Engine
{
	void ImGUI::Begin(const char* name, bool* p_open)
	{
		ImGui::Begin(name, p_open);
	}

	void ImGUI::ColorEdit3(const char* label, float col[3])
	{
		ImGui::ColorEdit3(label, col);
	}

	void ImGUI::End()
	{
		ImGui::End();
	}

	void ImGUI::SliderInt(const char* label, int* v, int v_min, int v_max)
	{
		ImGui::SliderInt(label, v, v_min, v_max);
	}

	void ImGUI::SliderInt2(const char* label, int v[2], int v_min, int v_max)
	{
		ImGui::SliderInt2(label, v, v_min, v_max);
	}

	void ImGUI::SliderInt3(const char* label, int v[3], int v_min, int v_max)
	{
		ImGui::SliderInt3(label, v, v_min, v_max);
	}

	void ImGUI::SliderInt4(const char* label, int v[4], int v_min, int v_max)
	{
		ImGui::SliderInt4(label, v, v_min, v_max);
	}

	void ImGUI::Checkbox(const char* label, bool* v)
	{
		ImGui::Checkbox(label, v);
	}

	void ImGUI::SliderFloat(const char* label, float* v, float v_min, float v_max)
	{
		ImGui::SliderFloat(label, v, v_min, v_max);
	}

	void ImGUI::SliderFloat2(const char* label, float v[2], float v_min, float v_max)
	{
		ImGui::SliderFloat2(label, v, v_min, v_max);
	}

	void ImGUI::SliderFloat3(const char* label, float v[3], float v_min, float v_max)
	{
		ImGui::SliderFloat3(label, v, v_min, v_max);
	}

	void ImGUI::SliderFloat4(const char* label, float v[4], float v_min, float v_max)
	{
		ImGui::SliderFloat4(label, v, v_min, v_max);
	}

	void ImGUI::ColorEdit4(const char* label, float col[4])
	{
		ImGui::ColorEdit4(label, col);
	}

	void ImGUI::ColorPicker3(const char* label, float col[3])
	{
		ImGui::ColorPicker3(label, col);
	}

	void ImGUI::ColorPicker4(const char* label, float col[4])
	{
		ImGui::ColorPicker4(label, col);
	}

	void ImGUI::DragFloat(const char* label, float* v, float v_speed, float v_min, float v_max)
	{
		ImGui::DragFloat(label, v, v_speed, v_min, v_max);
	}

	void ImGUI::DragFloat2(const char* label, float v[2], float v_speed, float v_min, float v_max)
	{
		ImGui::DragFloat2(label, v, v_speed, v_min, v_max);
	}

	void ImGUI::DragFloat3(const char* label, float v[3], float v_speed, float v_min, float v_max)
	{
		ImGui::DragFloat3(label, v, v_speed, v_min, v_max);
	}

	void ImGUI::DragFloat4(const char* label, float v[4], float v_speed, float v_min, float v_max)
	{
		ImGui::DragFloat4(label, v, v_speed, v_min, v_max);
	}

	void ImGUI::DragInt(const char* label, int* v, float v_speed, int v_min, int v_max)
	{
		ImGui::DragInt(label, v, v_speed, v_min, v_max);
	}

	void ImGUI::DragInt2(const char* label, int v[2], float v_speed, int v_min, int v_max)
	{
		ImGui::DragInt2(label, v, v_speed, v_min, v_max);
	}

	void ImGUI::DragInt3(const char* label, int v[3], float v_speed, int v_min, int v_max)
	{
		ImGui::DragInt3(label, v, v_speed, v_min, v_max);
	}

	void ImGUI::DragInt4(const char* label, int v[4], float v_speed, int v_min, int v_max)
	{
		ImGui::DragInt4(label, v, v_speed, v_min, v_max);
	}

	void ImGUI::SliderAngle(const char* label, float* v_rad, float v_degrees_min, float v_degrees_max)
	{
		ImGui::SliderAngle(label, v_rad, v_degrees_min, v_degrees_max);
	}
}