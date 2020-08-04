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

	public enum class Font
	{
		Verdana
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
	};
}