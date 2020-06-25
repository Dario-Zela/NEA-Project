#include "ENPH.h"
#include "OrthographicCameraController.h"
#include "Engine/Core/KeyCodes.h"
#include "Engine/Core/Input.h"

namespace Engine
{
	OrthographicCameraController::OrthographicCameraController(float aspectRatio, bool rotation)
		:mAspectRatio(aspectRatio), mCamera(-mAspectRatio * mZoomLevel, mAspectRatio* mZoomLevel, -mZoomLevel, mZoomLevel), mRotation(rotation) { }

	void OrthographicCameraController::OnUpdate(TimeStep timeStep)
	{
		if (Input::IsKeyPressed(EN_KEY_A)) 
		{
			mCameraPosition.x -= cos(glm::radians(mCameraRotation)) * mCameraSpeed * timeStep;
			mCameraPosition.y -= sin(glm::radians(mCameraRotation)) * mCameraSpeed * timeStep;
		}

		if (Input::IsKeyPressed(EN_KEY_W))
		{
			mCameraPosition.x -= sin(glm::radians(mCameraRotation)) * mCameraSpeed * timeStep;
			mCameraPosition.y += cos(glm::radians(mCameraRotation)) * mCameraSpeed * timeStep;
		}

		if (Input::IsKeyPressed(EN_KEY_D))
		{
			mCameraPosition.x += cos(glm::radians(mCameraRotation)) * mCameraSpeed * timeStep;
			mCameraPosition.y += sin(glm::radians(mCameraRotation)) * mCameraSpeed * timeStep;
		}

		if (Input::IsKeyPressed(EN_KEY_S))
		{
			mCameraPosition.x += sin(glm::radians(mCameraRotation)) * mCameraSpeed * timeStep;
			mCameraPosition.y -= cos(glm::radians(mCameraRotation)) * mCameraSpeed * timeStep;
		}
		
		if (mRotation)
		{
			if (Input::IsKeyPressed(EN_KEY_Q))
				mCameraRotation -= mCameraRotationSpeed * timeStep;

			if (Input::IsKeyPressed(EN_KEY_E))
				mCameraRotation += mCameraRotationSpeed * timeStep;
			if (mCameraRotation > 180.0f)
				mCameraRotation -= 360.0f;
			else if (mCameraRotation <= 180.0f)
				mCameraRotation += 360.0f;

			mCamera.SetRotation(mCameraRotation);
		}
		mCameraSpeed = mZoomLevel;
		mCamera.SetPosition(mCameraPosition);
	}

	void OrthographicCameraController::OnEvent(Event& e)
	{
		EventDispatcher dispatcher(e);
		dispatcher.Dispatch<MouseScrolledEvent>(EN_BIND_EVENT_FN(OrthographicCameraController::OnMouseScrolled));
		dispatcher.Dispatch<WindowResizeEvent>(EN_BIND_EVENT_FN(OrthographicCameraController::OnWindowResize));
	}

	bool OrthographicCameraController::OnMouseScrolled(MouseScrolledEvent& e)
	{
		mZoomLevel -= e.GetMouseYOffset() * 0.25f;
		mZoomLevel = std::max(mZoomLevel, 0.25f);
		mCamera.SetProjection(-mAspectRatio * mZoomLevel, mAspectRatio * mZoomLevel, -mZoomLevel, mZoomLevel);
		return false;
	}

	bool OrthographicCameraController::OnWindowResize(WindowResizeEvent& e)
	{
		mAspectRatio = (float)e.GetWidth() / (float)e.GetHeight();
		mCamera.SetProjection(-mAspectRatio * mZoomLevel, mAspectRatio * mZoomLevel, -mZoomLevel, mZoomLevel);
		return false;
	}

}