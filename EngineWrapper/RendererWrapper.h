#pragma once

#include "OpenGL/OpenGLTexture.h"

namespace Wrapper
{
	public ref class OrthographicCamera : public ManagedObject<Engine::OrthographicCamera>
	{
	public:
		OrthographicCamera(float left, float right, float bottom, float top) : ManagedObject(new Engine::OrthographicCamera(left, right, bottom, top)) {}
		OrthographicCamera(Engine::OrthographicCamera* camera) : ManagedObject(camera) {}

		void SetProjection(float left, float right, float bottom, float top) { mInstance->SetProjection(left, right, bottom, top); }
		void SetPosition(Vec3^ position) { mInstance->SetPosition(*position->GetInstance()); }
		void SetRotation(float rotation) { mInstance->SetRotation(rotation); }

		Vec3^ GetPosition() { auto var = mInstance->GetPosition(); return gcnew Vec3(&var); }
		float GetRotation() { return mInstance->GetRotation(); }

		Mat4^ GetProjectionMatrix() { auto var = mInstance->GetProjectionMatrix(); return gcnew Mat4(&var); }
		Mat4^ GetViewMatrix() { auto var = mInstance->GetViewMatrix(); return gcnew Mat4(&var); }
		Mat4^ GetViewProjectionMatrix() { auto var = mInstance->GetViewProjectionMatrix(); return gcnew Mat4(&var); }
	};

	public ref struct Renderer
	{
		static void Init() { Engine::Renderer::Init(); }
		static void OnWindowResise(unsigned int width, unsigned int height) { Engine::Renderer::OnWindowResise(width, height); }
		static void Shutdown() { Engine::Renderer::Shutdown(); }

		static void BeginScene(OrthographicCamera^ camera) { Engine::Renderer::BeginScene(*camera->GetInstance()); }
		static void Submit(VertexArray^ vertexArray, Shader^ shader, Mat4^ transform) 
		{
			Engine::Renderer::Submit(Engine::Ref<Engine::VertexArray>(vertexArray->GetInstance()), Engine::Ref<Engine::Shader>(shader->GetInstance()),
				*transform->GetInstance());
		}
		static void Submit(VertexArray^ vertexArray, Shader^ shader)
		{
			Engine::Renderer::Submit(Engine::Ref<Engine::VertexArray>(vertexArray->GetInstance()), Engine::Ref<Engine::Shader>(shader->GetInstance()));
		}
		
		static void EndScene() { Engine::Renderer::EndScene(); }

		static void Clear(Vec4^ color) { Engine::RenderCommand::Clear(*color->GetInstance()); }
		static void Clear() { Engine::RenderCommand::Clear(); }
	};
	
	public ref class Texture2D
	{
	private:
		Engine::OpenGLTexture2D* mInstance;
	public:
		Texture2D(String^ path) : mInstance(new Engine::OpenGLTexture2D(stringsToCStrings(path))) {}
		Texture2D(unsigned int width, unsigned int height) : mInstance(new Engine::OpenGLTexture2D(width, height)){}

		~Texture2D() { this->!Texture2D(); }
		!Texture2D() { mInstance->~OpenGLTexture2D(); }

		Engine::Texture2D* GetInstance() { return mInstance; }

		unsigned int GetWidth() { return mInstance->GetWidth(); }
		unsigned int GetHeight() { return mInstance->GetHeight(); }
		void SetData(void* data) { mInstance->SetData(data); }
		bool operator==(Texture2D^ other) { return this->GetInstance() == other->GetInstance(); }
		void Bind(unsigned int textureSlot) { mInstance->Bind(textureSlot); }
		void Bind() { mInstance->Bind(); }

		Engine::Texture2D* GetTexture()
		{
			return (Engine::Texture2D*)mInstance;
		}
	};
	
    public ref struct Colors
	{	
		static Vec4^ Void = gcnew Vec4(0.0f);
		static Vec4^ Maroon = gcnew Vec4(128 / 255.0f, 0 / 255.0f, 0 / 255.0f, 1.0f);
		static Vec4^ Dark_Red = gcnew Vec4(139 / 255.0f, 0 / 255.0f, 0 / 255.0f, 1.0f);
		static Vec4^ Brown = gcnew Vec4(165 / 255.0f, 42 / 255.0f, 42 / 255.0f, 1.0f);
		static Vec4^ Firebrick = gcnew Vec4(178 / 255.0f, 34 / 255.0f, 34 / 255.0f, 1.0f);
		static Vec4^ Crimson = gcnew Vec4(220 / 255.0f, 20 / 255.0f, 60 / 255.0f, 1.0f);
		static Vec4^ Red = gcnew Vec4(255 / 255.0f, 0 / 255.0f, 0 / 255.0f, 1.0f);
		static Vec4^ Tomato = gcnew Vec4(255 / 255.0f, 99 / 255.0f, 71 / 255.0f, 1.0f);
		static Vec4^ Coral = gcnew Vec4(255 / 255.0f, 127 / 255.0f, 80 / 255.0f, 1.0f);
		static Vec4^ Indian_Red = gcnew Vec4(205 / 255.0f, 92 / 255.0f, 92 / 255.0f, 1.0f);
		static Vec4^ Light_Coral = gcnew Vec4(240 / 255.0f, 128 / 255.0f, 128 / 255.0f, 1.0f);
		static Vec4^ Dark_Salmon = gcnew Vec4(233 / 255.0f, 150 / 255.0f, 122 / 255.0f, 1.0f);
		static Vec4^ Salmon = gcnew Vec4(250 / 255.0f, 128 / 255.0f, 114 / 255.0f, 1.0f);
		static Vec4^ Light_Salmon = gcnew Vec4(255 / 255.0f, 160 / 255.0f, 122 / 255.0f, 1.0f);
		static Vec4^ Orange_Red = gcnew Vec4(255 / 255.0f, 69 / 255.0f, 0 / 255.0f, 1.0f);
		static Vec4^ Dark_Orange = gcnew Vec4(255 / 255.0f, 140 / 255.0f, 0 / 255.0f, 1.0f);
		static Vec4^ Orange = gcnew Vec4(255 / 255.0f, 165 / 255.0f, 0 / 255.0f, 1.0f);
		static Vec4^ Gold = gcnew Vec4(255 / 255.0f, 215 / 255.0f, 0 / 255.0f, 1.0f);
		static Vec4^ Dark_Golden_Rod = gcnew Vec4(184 / 255.0f, 134 / 255.0f, 11 / 255.0f, 1.0f);
		static Vec4^ Golden_Rod = gcnew Vec4(218 / 255.0f, 165 / 255.0f, 32 / 255.0f, 1.0f);
		static Vec4^ Pale_Golden_Rod = gcnew Vec4(238 / 255.0f, 232 / 255.0f, 170 / 255.0f, 1.0f);
		static Vec4^ Dark_Khaki = gcnew Vec4(189 / 255.0f, 183 / 255.0f, 107 / 255.0f, 1.0f);
		static Vec4^ Khaki = gcnew Vec4(240 / 255.0f, 230 / 255.0f, 140 / 255.0f, 1.0f);
		static Vec4^ Olive = gcnew Vec4(128 / 255.0f, 128 / 255.0f, 0 / 255.0f, 1.0f);
		static Vec4^ Yellow = gcnew Vec4(255 / 255.0f, 255 / 255.0f, 0 / 255.0f, 1.0f);
		static Vec4^ Yellow_Green = gcnew Vec4(154 / 255.0f, 205 / 255.0f, 50 / 255.0f, 1.0f);
		static Vec4^ Dark_Olive_Green = gcnew Vec4(85 / 255.0f, 107 / 255.0f, 47 / 255.0f, 1.0f);
		static Vec4^ Olive_Drab = gcnew Vec4(107 / 255.0f, 142 / 255.0f, 35 / 255.0f, 1.0f);
		static Vec4^ Lawn_Green = gcnew Vec4(124 / 255.0f, 252 / 255.0f, 0 / 255.0f, 1.0f);
		static Vec4^ Chart_Reuse = gcnew Vec4(127 / 255.0f, 255 / 255.0f, 0 / 255.0f, 1.0f);
		static Vec4^ Green_Yellow = gcnew Vec4(173 / 255.0f, 255 / 255.0f, 47 / 255.0f, 1.0f);
		static Vec4^ Dark_Green = gcnew Vec4(0 / 255.0f, 100 / 255.0f, 0 / 255.0f, 1.0f);
		static Vec4^ Green = gcnew Vec4(0 / 255.0f, 128 / 255.0f, 0 / 255.0f, 1.0f);
		static Vec4^ Forest_Green = gcnew Vec4(34 / 255.0f, 139 / 255.0f, 34 / 255.0f, 1.0f);
		static Vec4^ Lime = gcnew Vec4(0 / 255.0f, 255 / 255.0f, 0 / 255.0f, 1.0f);
		static Vec4^ Lime_Green = gcnew Vec4(50 / 255.0f, 205 / 255.0f, 50 / 255.0f, 1.0f);
		static Vec4^ Light_Green = gcnew Vec4(144 / 255.0f, 238 / 255.0f, 144 / 255.0f, 1.0f);
		static Vec4^ Pale_Green = gcnew Vec4(152 / 255.0f, 251 / 255.0f, 152 / 255.0f, 1.0f);
		static Vec4^ Dark_Sea_Green = gcnew Vec4(143 / 255.0f, 188 / 255.0f, 143 / 255.0f, 1.0f);
		static Vec4^ Medium_Spring_Green = gcnew Vec4(0 / 255.0f, 250 / 255.0f, 154 / 255.0f, 1.0f);
		static Vec4^ Spring_Green = gcnew Vec4(0 / 255.0f, 255 / 255.0f, 127 / 255.0f, 1.0f);
		static Vec4^ Sea_Green = gcnew Vec4(46 / 255.0f, 139 / 255.0f, 87 / 255.0f, 1.0f);
		static Vec4^ Medium_Aqua_Marine = gcnew Vec4(102 / 255.0f, 205 / 255.0f, 170 / 255.0f, 1.0f);
		static Vec4^ Medium_Sea_Green = gcnew Vec4(60 / 255.0f, 179 / 255.0f, 113 / 255.0f, 1.0f);
		static Vec4^ Light_Sea_Green = gcnew Vec4(32 / 255.0f, 178 / 255.0f, 170 / 255.0f, 1.0f);
		static Vec4^ Dark_Slate_Gray = gcnew Vec4(47 / 255.0f, 79 / 255.0f, 79 / 255.0f, 1.0f);
		static Vec4^ Teal = gcnew Vec4(0 / 255.0f, 128 / 255.0f, 128 / 255.0f, 1.0f);
		static Vec4^ Dark_Cyan = gcnew Vec4(0 / 255.0f, 139 / 255.0f, 139 / 255.0f, 1.0f);
		static Vec4^ Aqua = gcnew Vec4(0 / 255.0f, 255 / 255.0f, 255 / 255.0f, 1.0f);
		static Vec4^ Cyan = gcnew Vec4(0 / 255.0f, 255 / 255.0f, 255 / 255.0f, 1.0f);
		static Vec4^ Light_Cyan = gcnew Vec4(224 / 255.0f, 255 / 255.0f, 255 / 255.0f, 1.0f);
		static Vec4^ Dark_Turquoise = gcnew Vec4(0 / 255.0f, 206 / 255.0f, 209 / 255.0f, 1.0f);
		static Vec4^ Turquoise = gcnew Vec4(64 / 255.0f, 224 / 255.0f, 208 / 255.0f, 1.0f);
		static Vec4^ Medium_Turquoise = gcnew Vec4(72 / 255.0f, 209 / 255.0f, 204 / 255.0f, 1.0f);
		static Vec4^ Pale_Turquoise = gcnew Vec4(175 / 255.0f, 238 / 255.0f, 238 / 255.0f, 1.0f);
		static Vec4^ Aqua_Marine = gcnew Vec4(127 / 255.0f, 255 / 255.0f, 212 / 255.0f, 1.0f);
		static Vec4^ Powder_Blue = gcnew Vec4(176 / 255.0f, 224 / 255.0f, 230 / 255.0f, 1.0f);
		static Vec4^ Cadet_Blue = gcnew Vec4(95 / 255.0f, 158 / 255.0f, 160 / 255.0f, 1.0f);
		static Vec4^ Steel_Blue = gcnew Vec4(70 / 255.0f, 130 / 255.0f, 180 / 255.0f, 1.0f);
		static Vec4^ Corn_Flower_Blue = gcnew Vec4(100 / 255.0f, 149 / 255.0f, 237 / 255.0f, 1.0f);
		static Vec4^ Deep_Sky_Blue = gcnew Vec4(0 / 255.0f, 191 / 255.0f, 255 / 255.0f, 1.0f);
		static Vec4^ Dodger_Blue = gcnew Vec4(30 / 255.0f, 144 / 255.0f, 255 / 255.0f, 1.0f);
		static Vec4^ Light_Blue = gcnew Vec4(173 / 255.0f, 216 / 255.0f, 230 / 255.0f, 1.0f);
		static Vec4^ Sky_Blue = gcnew Vec4(135 / 255.0f, 206 / 255.0f, 235 / 255.0f, 1.0f);
		static Vec4^ Light_Sky_Blue = gcnew Vec4(135 / 255.0f, 206 / 255.0f, 250 / 255.0f, 1.0f);
		static Vec4^ Midnight_Blue = gcnew Vec4(25 / 255.0f, 25 / 255.0f, 112 / 255.0f, 1.0f);
		static Vec4^ Navy = gcnew Vec4(0 / 255.0f, 0 / 255.0f, 128 / 255.0f, 1.0f);
		static Vec4^ Dark_Blue = gcnew Vec4(0 / 255.0f, 0 / 255.0f, 139 / 255.0f, 1.0f);
		static Vec4^ Medium_Blue = gcnew Vec4(0 / 255.0f, 0 / 255.0f, 205 / 255.0f, 1.0f);
		static Vec4^ Blue = gcnew Vec4(0 / 255.0f, 0 / 255.0f, 255 / 255.0f, 1.0f);
		static Vec4^ Royal_Blue = gcnew Vec4(65 / 255.0f, 105 / 255.0f, 225 / 255.0f, 1.0f);
		static Vec4^ Blue_Violet = gcnew Vec4(138 / 255.0f, 43 / 255.0f, 226 / 255.0f, 1.0f);
		static Vec4^ Indigo = gcnew Vec4(75 / 255.0f, 0 / 255.0f, 130 / 255.0f, 1.0f);
		static Vec4^ Dark_Slate_Blue = gcnew Vec4(72 / 255.0f, 61 / 255.0f, 139 / 255.0f, 1.0f);
		static Vec4^ Slate_Blue = gcnew Vec4(106 / 255.0f, 90 / 255.0f, 205 / 255.0f, 1.0f);
		static Vec4^ Medium_Slate_Blue = gcnew Vec4(123 / 255.0f, 104 / 255.0f, 238 / 255.0f, 1.0f);
		static Vec4^ Medium_Purple = gcnew Vec4(147 / 255.0f, 112 / 255.0f, 219 / 255.0f, 1.0f);
		static Vec4^ Dark_Magenta = gcnew Vec4(139 / 255.0f, 0 / 255.0f, 139 / 255.0f, 1.0f);
		static Vec4^ Dark_Violet = gcnew Vec4(148 / 255.0f, 0 / 255.0f, 211 / 255.0f, 1.0f);
		static Vec4^ Dark_Orchid = gcnew Vec4(153 / 255.0f, 50 / 255.0f, 204 / 255.0f, 1.0f);
		static Vec4^ Medium_Orchid = gcnew Vec4(186 / 255.0f, 85 / 255.0f, 211 / 255.0f, 1.0f);
		static Vec4^ Purple = gcnew Vec4(128 / 255.0f, 0 / 255.0f, 128 / 255.0f, 1.0f);
		static Vec4^ Thistle = gcnew Vec4(216 / 255.0f, 191 / 255.0f, 216 / 255.0f, 1.0f);
		static Vec4^ Plum = gcnew Vec4(221 / 255.0f, 160 / 255.0f, 221 / 255.0f, 1.0f);
		static Vec4^ Violet = gcnew Vec4(238 / 255.0f, 130 / 255.0f, 238 / 255.0f, 1.0f);
		static Vec4^ Magenta = gcnew Vec4(255 / 255.0f, 0 / 255.0f, 255 / 255.0f, 1.0f);
		static Vec4^ Orchid = gcnew Vec4(218 / 255.0f, 112 / 255.0f, 214 / 255.0f, 1.0f);
		static Vec4^ Medium_Violet_Red = gcnew Vec4(199 / 255.0f, 21 / 255.0f, 133 / 255.0f, 1.0f);
		static Vec4^ Pale_Violet_Red = gcnew Vec4(219 / 255.0f, 112 / 255.0f, 147 / 255.0f, 1.0f);
		static Vec4^ Deep_Pink = gcnew Vec4(255 / 255.0f, 20 / 255.0f, 147 / 255.0f, 1.0f);
		static Vec4^ Hot_Pink = gcnew Vec4(255 / 255.0f, 105 / 255.0f, 180 / 255.0f, 1.0f);
		static Vec4^ Light_Pink = gcnew Vec4(255 / 255.0f, 182 / 255.0f, 193 / 255.0f, 1.0f);
		static Vec4^ Pink = gcnew Vec4(255 / 255.0f, 192 / 255.0f, 203 / 255.0f, 1.0f);
		static Vec4^ Antique_White = gcnew Vec4(250 / 255.0f, 235 / 255.0f, 215 / 255.0f, 1.0f);
		static Vec4^ Beige = gcnew Vec4(245 / 255.0f, 245 / 255.0f, 220 / 255.0f, 1.0f);
		static Vec4^ Bisque = gcnew Vec4(255 / 255.0f, 228 / 255.0f, 196 / 255.0f, 1.0f);
		static Vec4^ Blanched_Almond = gcnew Vec4(255 / 255.0f, 235 / 255.0f, 205 / 255.0f, 1.0f);
		static Vec4^ Wheat = gcnew Vec4(245 / 255.0f, 222 / 255.0f, 179 / 255.0f, 1.0f);
		static Vec4^ Corn_Silk = gcnew Vec4(255 / 255.0f, 248 / 255.0f, 220 / 255.0f, 1.0f);
		static Vec4^ Lemon_Chiffon = gcnew Vec4(255 / 255.0f, 250 / 255.0f, 205 / 255.0f, 1.0f);
		static Vec4^ Light_Golden_Rod_Yellow = gcnew Vec4(250 / 255.0f, 250 / 255.0f, 210 / 255.0f, 1.0f);
		static Vec4^ Light_Yellow = gcnew Vec4(255 / 255.0f, 255 / 255.0f, 224 / 255.0f, 1.0f);
		static Vec4^ Saddle_Brown = gcnew Vec4(139 / 255.0f, 69 / 255.0f, 19 / 255.0f, 1.0f);
		static Vec4^ Sienna = gcnew Vec4(160 / 255.0f, 82 / 255.0f, 45 / 255.0f, 1.0f);
		static Vec4^ Chocolate = gcnew Vec4(210 / 255.0f, 105 / 255.0f, 30 / 255.0f, 1.0f);
		static Vec4^ Peru = gcnew Vec4(205 / 255.0f, 133 / 255.0f, 63 / 255.0f, 1.0f);
		static Vec4^ Sandy_Brown = gcnew Vec4(244 / 255.0f, 164 / 255.0f, 96 / 255.0f, 1.0f);
		static Vec4^ Burly_Wood = gcnew Vec4(222 / 255.0f, 184 / 255.0f, 135 / 255.0f, 1.0f);
		static Vec4^ Tan = gcnew Vec4(210 / 255.0f, 180 / 255.0f, 140 / 255.0f, 1.0f);
		static Vec4^ Rosy_Brown = gcnew Vec4(188 / 255.0f, 143 / 255.0f, 143 / 255.0f, 1.0f);
		static Vec4^ Moccasin = gcnew Vec4(255 / 255.0f, 228 / 255.0f, 181 / 255.0f, 1.0f);
		static Vec4^ Navajo_White = gcnew Vec4(255 / 255.0f, 222 / 255.0f, 173 / 255.0f, 1.0f);
		static Vec4^ Peach_Puff = gcnew Vec4(255 / 255.0f, 218 / 255.0f, 185 / 255.0f, 1.0f);
		static Vec4^ Misty_Rose = gcnew Vec4(255 / 255.0f, 228 / 255.0f, 225 / 255.0f, 1.0f);
		static Vec4^ Lavender_Blush = gcnew Vec4(255 / 255.0f, 240 / 255.0f, 245 / 255.0f, 1.0f);
		static Vec4^ Linen = gcnew Vec4(250 / 255.0f, 240 / 255.0f, 230 / 255.0f, 1.0f);
		static Vec4^ Old_Lace = gcnew Vec4(253 / 255.0f, 245 / 255.0f, 230 / 255.0f, 1.0f);
		static Vec4^ Papaya_Whip = gcnew Vec4(255 / 255.0f, 239 / 255.0f, 213 / 255.0f, 1.0f);
		static Vec4^ Sea_Shell = gcnew Vec4(255 / 255.0f, 245 / 255.0f, 238 / 255.0f, 1.0f);
		static Vec4^ Mint_Cream = gcnew Vec4(245 / 255.0f, 255 / 255.0f, 250 / 255.0f, 1.0f);
		static Vec4^ Slate_Gray = gcnew Vec4(112 / 255.0f, 128 / 255.0f, 144 / 255.0f, 1.0f);
		static Vec4^ Light_Slate_Gray = gcnew Vec4(119 / 255.0f, 136 / 255.0f, 153 / 255.0f, 1.0f);
		static Vec4^ Light_Steel_Blue = gcnew Vec4(176 / 255.0f, 196 / 255.0f, 222 / 255.0f, 1.0f);
		static Vec4^ Lavender = gcnew Vec4(230 / 255.0f, 230 / 255.0f, 250 / 255.0f, 1.0f);
		static Vec4^ Floral_White = gcnew Vec4(255 / 255.0f, 250 / 255.0f, 240 / 255.0f, 1.0f);
		static Vec4^ Alice_Blue = gcnew Vec4(240 / 255.0f, 248 / 255.0f, 255 / 255.0f, 1.0f);
		static Vec4^ Ghost_White = gcnew Vec4(248 / 255.0f, 248 / 255.0f, 255 / 255.0f, 1.0f);
		static Vec4^ Honeydew = gcnew Vec4(240 / 255.0f, 255 / 255.0f, 240 / 255.0f, 1.0f);
		static Vec4^ Ivory = gcnew Vec4(255 / 255.0f, 255 / 255.0f, 240 / 255.0f, 1.0f);
		static Vec4^ Azure = gcnew Vec4(240 / 255.0f, 255 / 255.0f, 255 / 255.0f, 1.0f);
		static Vec4^ Snow = gcnew Vec4(255 / 255.0f, 250 / 255.0f, 250 / 255.0f, 1.0f);
		static Vec4^ Black = gcnew Vec4(0 / 255.0f, 0 / 255.0f, 0 / 255.0f, 1.0f);
		static Vec4^ Dim_Gray = gcnew Vec4(105 / 255.0f, 105 / 255.0f, 105 / 255.0f, 1.0f);
		static Vec4^ Gray = gcnew Vec4(128 / 255.0f, 128 / 255.0f, 128 / 255.0f, 1.0f);
		static Vec4^ Dark_Gray = gcnew Vec4(169 / 255.0f, 169 / 255.0f, 169 / 255.0f, 1.0f);
		static Vec4^ Silver = gcnew Vec4(192 / 255.0f, 192 / 255.0f, 192 / 255.0f, 1.0f);
		static Vec4^ Light_Gray = gcnew Vec4(211 / 255.0f, 211 / 255.0f, 211 / 255.0f, 1.0f);
		static Vec4^ Gainsboro = gcnew Vec4(220 / 255.0f, 220 / 255.0f, 220 / 255.0f, 1.0f);
		static Vec4^ White_Smoke = gcnew Vec4(245 / 255.0f, 245 / 255.0f, 245 / 255.0f, 1.0f);
		static Vec4^ White = gcnew Vec4(255 / 255.0f, 255 / 255.0f, 255 / 255.0f, 1.0f);
	};

	public enum class Font
	{
		Verdana, TimesNewRoman, FixedDsys
	};

	public ref struct Renderer2D
	{
		static void Init() { Engine::Renderer2D::Init(); }
		static void Shutdown() { Engine::Renderer2D::Shutdown(); }

		static void BeginScene(OrthographicCamera^ camera) { Engine::Renderer2D::BeginScene(*camera->GetInstance()); }
		static void EndScene() { Engine::Renderer2D::EndScene(); }
		static void Flush() { Engine::Renderer2D::Flush(); }

		static void DrawQuad(Vec2^ position, Vec2^ size, Vec4^ color) { Engine::Renderer2D::DrawQuad(*position->GetInstance(), *size->GetInstance(), *color->GetInstance()); }
		static void DrawQuad(Vec2^ position, Vec2^ size, Vec4^ color, float rotation) { Engine::Renderer2D::DrawQuad(*position->GetInstance(), *size->GetInstance(), *color->GetInstance(), rotation); }
		static void DrawQuad(Vec3^ position, Vec2^ size, Vec4^ color) { Engine::Renderer2D::DrawQuad(*position->GetInstance(), *size->GetInstance(), *color->GetInstance()); }
		static void DrawQuad(Vec3^ position, Vec2^ size, Vec4^ color, float rotation) { Engine::Renderer2D::DrawQuad(*position->GetInstance(), *size->GetInstance(), *color->GetInstance(), rotation); }

		static void DrawQuad(Vec2^ position, Vec2^ size, Texture2D^ texture) { Engine::Renderer2D::DrawQuad(*position->GetInstance(), *size->GetInstance(), *texture->GetTexture()); }
		static void DrawQuad(Vec2^ position, Vec2^ size, Texture2D^ texture, Vec4^ shade) { Engine::Renderer2D::DrawQuad(*position->GetInstance(), *size->GetInstance(), *texture->GetTexture(), *shade->GetInstance()); }
		static void DrawQuad(Vec2^ position, Vec2^ size, Texture2D^ texture, Vec4^ shade, float textureScale) { Engine::Renderer2D::DrawQuad(*position->GetInstance(), *size->GetInstance(), *texture->GetTexture(), *shade->GetInstance(), textureScale); }
		static void DrawQuad(Vec2^ position, Vec2^ size, Texture2D^ texture, Vec4^ shade, float textureScale, float rotation) { Engine::Renderer2D::DrawQuad(*position->GetInstance(), *size->GetInstance(), *texture->GetTexture(), *shade->GetInstance(), textureScale, rotation); }

		static void DrawQuad(Vec3^ position, Vec2^ size, Texture2D^ texture) { Engine::Renderer2D::DrawQuad(*position->GetInstance(), *size->GetInstance(), *texture->GetTexture()); }
		static void DrawQuad(Vec3^ position, Vec2^ size, Texture2D^ texture, Vec4^ shade) { Engine::Renderer2D::DrawQuad(*position->GetInstance(), *size->GetInstance(), *texture->GetTexture(), *shade->GetInstance()); }
		static void DrawQuad(Vec3^ position, Vec2^ size, Texture2D^ texture, Vec4^ shade, float textureScale) { Engine::Renderer2D::DrawQuad(*position->GetInstance(), *size->GetInstance(), *texture->GetTexture(), *shade->GetInstance(), textureScale); }
		static void DrawQuad(Vec3^ position, Vec2^ size, Texture2D^ texture, Vec4^ shade, float textureScale, float rotation) { Engine::Renderer2D::DrawQuad(*position->GetInstance(), *size->GetInstance(), *texture->GetTexture(), *shade->GetInstance(), textureScale, rotation); }

		static void DrawText(String^ text, Vec2^ position, Vec2^ size, Font^ font) { Engine::Renderer2D::DrawText(stringsToCStrings(text), *position->GetInstance(), *size->GetInstance(), (Engine::Font)(int)*font); }
		static void DrawText(String^ text, Vec2^ position, Vec2^ size, Font^ font, Vec4^ shade) { Engine::Renderer2D::DrawText(stringsToCStrings(text), *position->GetInstance(), *size->GetInstance(), (Engine::Font)(int) * font, *shade->GetInstance()); }
		static void DrawText(String^ text, Vec2^ position, Vec2^ size, Font^ font, Vec4^ shade, float rotation) { Engine::Renderer2D::DrawText(stringsToCStrings(text), *position->GetInstance(), *size->GetInstance(), (Engine::Font)(int) * font, *shade->GetInstance(), rotation); }

		static void DrawText(String^ text, Vec3^ position, Vec2^ size, Font^ font) { Engine::Renderer2D::DrawText(stringsToCStrings(text), *position->GetInstance(), *size->GetInstance(), (Engine::Font)(int) * font); }
		static void DrawText(String^ text, Vec3^ position, Vec2^ size, Font^ font, Vec4^ shade) { Engine::Renderer2D::DrawText(stringsToCStrings(text), *position->GetInstance(), *size->GetInstance(), (Engine::Font)(int) * font, *shade->GetInstance()); }
		static void DrawText(String^ text, Vec3^ position, Vec2^ size, Font^ font, Vec4^ shade, float rotation) { Engine::Renderer2D::DrawText(stringsToCStrings(text), *position->GetInstance(), *size->GetInstance(), (Engine::Font)(int) * font, *shade->GetInstance(), rotation); }
	
		static void DrawLine(Vec2^ position1, Vec2^ position2, float thickness, Vec4^ color) { Engine::Renderer2D::DrawLine(*position1->GetInstance(), *position2->GetInstance(), thickness, *color->GetInstance()); }
		static void DrawRect(Vec2^ position1, Vec2^ position2, float thickness, Vec4^ color) { Engine::Renderer2D::DrawRect(*position1->GetInstance(), *position2->GetInstance(), thickness, *color->GetInstance()); }
		

	};
}