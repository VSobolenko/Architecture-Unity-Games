using System;
using CameraLogic;
using Data;
using Infrastructure;
using Infrastructure.Services;
using Services.Input;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Hero
{
public class HeroMove : MonoBehaviour, ISavedProgress
{
    public CharacterController characterController;
    public HeroAnimator heroAnimator;
    public float movementSpeed = 1f;
    private IInputService _inputService;
    private Camera _camera;

    private void Awake()
    {
        _inputService = AllServices.Container.Single<IInputService>();
    }

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        Vector3 movementVector = Vector3.zero;

        if (_inputService.Axis.sqrMagnitude > Constants.Epsilon)
        {
            movementVector = _camera.transform.TransformDirection(_inputService.Axis);
            movementVector.y = 0;
            movementVector.Normalize();

            transform.forward = movementVector;
        }

        movementVector += Physics.gravity;

        characterController.Move(movementSpeed * movementVector * Time.deltaTime);
        heroAnimator.UpdateVal(characterController.velocity.magnitude);
    }

    private void OnValidate()
    {
        if (characterController == null)
            characterController = GetComponent<CharacterController>();
    }

    public void UpdateProgress(PlayerProgress progress)
    {
        progress.worldData.positionOnLevel = new PositionOnLevel(CurrentLevel(), transform.position.AsVectorData());
    }

    public void LoadProgress(PlayerProgress progress)
    {
        if (CurrentLevel() == progress.worldData.positionOnLevel.level)
        {
            var savedPosition = progress.worldData.positionOnLevel.position;
            if (savedPosition != null)
                Warp(savedPosition);
        }
    }

    private void Warp(Vector3Data to)
    {
        characterController.enabled = false;
        transform.position = to.AsUnityVector().AddY(characterController.height);
        characterController.enabled = true;
    }

    private static string CurrentLevel() => SceneManager.GetActiveScene().name;
}
}