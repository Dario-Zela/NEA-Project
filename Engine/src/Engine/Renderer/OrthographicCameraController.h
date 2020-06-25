#pragma once

#include "Engine/Renderer/OrthographicCamera.h"
#include "Engine/Core/TimeStep.h"

#include "Engine/Events/ApplicationEvent.h"
#include "Engine/Events/MouseEvent.h"

namespace Engine
{
	class ENGINE_API OrthographicCameraController
	{
	public:
		OrthographicCameraController(float aspectRatio, bool rotation = false);

		void OnUpdate(TimeStep timeStep);
		void OnEvent(Event& e);

		inline OrthographicCamera& GetCamera() { return mCamera; }
		inline const OrthographicCamera& GetCamera() const { return mCamera; }
	private:
		bool OnMouseScrolled(MouseScrolledEvent& e);
		bool OnWindowResize(WindowResizeEvent& e);
		float mAspectRatio;
		float mZoomLevel = 1.0f;
		OrthographicCamera mCamera;

		bool mRotation;
		glm::vec3 mCameraPosition = glm::vec3(0.0f);
		float mCameraRotation = 0.0f, mCameraSpeed = 5.0f, mCameraRotationSpeed = 180.0f;
	};
}