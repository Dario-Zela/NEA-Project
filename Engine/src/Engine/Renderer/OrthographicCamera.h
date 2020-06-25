#pragma once
#include <glm/glm.hpp>

namespace Engine
{
	class ENGINE_API OrthographicCamera
	{
	public:
		OrthographicCamera(float left, float right, float bottom, float top);
		void SetProjection(float left, float right, float bottom, float top);

		inline void SetPosition(const glm::vec3& position) { mPosition = position; RecalculateViewMatrix();}
		inline void SetRotation(float rotation) { mRotation = rotation; RecalculateViewMatrix(); }
		
		inline const glm::vec3& GetPosition() const { return mPosition; }
		inline const float& GetRotation() const { return mRotation; }

		inline const glm::mat4& GetProjectionMatrix() const { return mProjectionMatrix; }
		inline const glm::mat4& GetViewMatrix() const { return mViewMatrix; }
		inline const glm::mat4& GetViewProjectionMatrix() const { return mViewProjectionMatrix; }
	private:
		void RecalculateViewMatrix();

		glm::mat4 mProjectionMatrix;
		glm::mat4 mViewMatrix;
		glm::mat4 mViewProjectionMatrix;

		glm::vec3 mPosition;
		float mRotation = 0.0f;
	};
}