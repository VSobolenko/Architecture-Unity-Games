using System;
using CameraLogic;
using Infrastructure;
using Infrastructure.Services;
using Services.Input;
using UnityEngine;

namespace Hero
{
public class HeroMove : MonoBehaviour
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
}
}