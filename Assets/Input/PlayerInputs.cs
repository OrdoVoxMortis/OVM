//------------------------------------------------------------------------------
// <auto-generated>
//     This code was auto-generated by com.unity.inputsystem:InputActionCodeGenerator
//     version 1.7.0
//     from Assets/Input/PlayerInputs.inputactions
//
//     Changes to this file may cause incorrect behavior and will be lost if
//     the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

public partial class @PlayerInputs: IInputActionCollection2, IDisposable
{
    public InputActionAsset asset { get; }
    public @PlayerInputs()
    {
        asset = InputActionAsset.FromJson(@"{
    ""name"": ""PlayerInputs"",
    ""maps"": [
        {
            ""name"": ""Player"",
            ""id"": ""932f6d7e-3f2d-436d-b6f8-71c306b04a24"",
            ""actions"": [
                {
                    ""name"": ""Movement"",
                    ""type"": ""Value"",
                    ""id"": ""f8249df4-d80a-4ce9-9bf9-152602aad2bc"",
                    ""expectedControlType"": ""Vector2"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Run"",
                    ""type"": ""Button"",
                    ""id"": ""60599d88-0da6-404d-b1a1-e9f4f02e359d"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": ""Press"",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Look"",
                    ""type"": ""Value"",
                    ""id"": ""a8e25f89-8c07-4d67-b060-6cf39457dcd2"",
                    ""expectedControlType"": ""Delta"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                },
                {
                    ""name"": ""Jump"",
                    ""type"": ""Button"",
                    ""id"": ""af81e309-984c-442a-ae50-ee07b5f2c6d8"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Squat"",
                    ""type"": ""Button"",
                    ""id"": ""2628fed5-caa8-41fb-99da-4c3c5e48ef93"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Interection"",
                    ""type"": ""Button"",
                    ""id"": ""0f242b9f-0c2e-48bf-95e0-b2ff15750e03"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Setting"",
                    ""type"": ""Button"",
                    ""id"": ""68694467-81c9-406e-b220-d2f78eda49a5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Simulate"",
                    ""type"": ""Button"",
                    ""id"": ""20f1bf21-4e2c-4966-bf23-6453a051d47b"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Accept"",
                    ""type"": ""Button"",
                    ""id"": ""0a468ef3-3dc6-4392-b76a-6fbd6e915fde"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""CancelUI"",
                    ""type"": ""Button"",
                    ""id"": ""c57b5392-46f6-4513-b1d6-c6fe193d42e5"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                },
                {
                    ""name"": ""Zoom"",
                    ""type"": ""Value"",
                    ""id"": ""fde73552-32fc-4351-b847-6db70ec59664"",
                    ""expectedControlType"": ""Axis"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": true
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""b2905e36-2b4b-49ce-93b2-29c71e489935"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Jump"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""2D Vector"",
                    ""id"": ""4f78335c-ac10-4963-95a1-b101080769a7"",
                    ""path"": ""2DVector"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": true,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": ""up"",
                    ""id"": ""9d3db669-4e57-4a54-8836-783dae2107ea"",
                    ""path"": ""<Keyboard>/w"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""down"",
                    ""id"": ""9b7be030-57d5-4888-8fdc-648b53937fe7"",
                    ""path"": ""<Keyboard>/s"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""left"",
                    ""id"": ""311a6878-142a-4a84-b0c5-b952e88a2049"",
                    ""path"": ""<Keyboard>/a"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": ""right"",
                    ""id"": ""18b07876-48bf-4951-8863-67f0da0c05f7"",
                    ""path"": ""<Keyboard>/d"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Movement"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": true
                },
                {
                    ""name"": """",
                    ""id"": ""145a1036-4114-4e93-a841-0cca7e61f088"",
                    ""path"": ""<Mouse>/delta"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Look"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""623835f1-6a33-4aa3-a638-169dc620e334"",
                    ""path"": ""<Keyboard>/leftShift"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Run"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""7a4df08a-de63-461e-a645-2dd771ac6a56"",
                    ""path"": ""<Keyboard>/v"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Squat"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""73b7cefa-c12a-4e7c-ae8b-766399a4c9f5"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Interection"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c745c948-982f-4a2a-8c22-57485525f8e5"",
                    ""path"": ""<Keyboard>/m"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Setting"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""96e2c7e9-990a-4425-aa4c-cb7c8fc7a099"",
                    ""path"": ""<Keyboard>/tab"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Simulate"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""c2607d03-cc67-417c-814c-be1d59b76d23"",
                    ""path"": ""<Keyboard>/escape"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""CancelUI"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""31175aec-60db-40dd-910b-0579b99012b5"",
                    ""path"": ""<Keyboard>/e"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Accept"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                },
                {
                    ""name"": """",
                    ""id"": ""8590a938-9c0f-4183-aba7-3d581efee674"",
                    ""path"": ""<Mouse>/scroll/y"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""Zoom"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        },
        {
            ""name"": ""RhythmMode"",
            ""id"": ""d5eecc6d-9063-4eb4-9d70-fcc8fd1574e3"",
            ""actions"": [
                {
                    ""name"": ""RhythmInput"",
                    ""type"": ""Button"",
                    ""id"": ""66c0fcaf-b53f-4aa8-823d-8a8b5f3f55b6"",
                    ""expectedControlType"": ""Button"",
                    ""processors"": """",
                    ""interactions"": """",
                    ""initialStateCheck"": false
                }
            ],
            ""bindings"": [
                {
                    ""name"": """",
                    ""id"": ""7a6f2c4c-1b3b-4ce9-b842-577b3e0206c6"",
                    ""path"": ""<Keyboard>/space"",
                    ""interactions"": """",
                    ""processors"": """",
                    ""groups"": """",
                    ""action"": ""RhythmInput"",
                    ""isComposite"": false,
                    ""isPartOfComposite"": false
                }
            ]
        }
    ],
    ""controlSchemes"": []
}");
        // Player
        m_Player = asset.FindActionMap("Player", throwIfNotFound: true);
        m_Player_Movement = m_Player.FindAction("Movement", throwIfNotFound: true);
        m_Player_Run = m_Player.FindAction("Run", throwIfNotFound: true);
        m_Player_Look = m_Player.FindAction("Look", throwIfNotFound: true);
        m_Player_Jump = m_Player.FindAction("Jump", throwIfNotFound: true);
        m_Player_Squat = m_Player.FindAction("Squat", throwIfNotFound: true);
        m_Player_Interection = m_Player.FindAction("Interection", throwIfNotFound: true);
        m_Player_Setting = m_Player.FindAction("Setting", throwIfNotFound: true);
        m_Player_Simulate = m_Player.FindAction("Simulate", throwIfNotFound: true);
        m_Player_Accept = m_Player.FindAction("Accept", throwIfNotFound: true);
        m_Player_CancelUI = m_Player.FindAction("CancelUI", throwIfNotFound: true);
        m_Player_Zoom = m_Player.FindAction("Zoom", throwIfNotFound: true);
        // RhythmMode
        m_RhythmMode = asset.FindActionMap("RhythmMode", throwIfNotFound: true);
        m_RhythmMode_RhythmInput = m_RhythmMode.FindAction("RhythmInput", throwIfNotFound: true);
    }

    public void Dispose()
    {
        UnityEngine.Object.Destroy(asset);
    }

    public InputBinding? bindingMask
    {
        get => asset.bindingMask;
        set => asset.bindingMask = value;
    }

    public ReadOnlyArray<InputDevice>? devices
    {
        get => asset.devices;
        set => asset.devices = value;
    }

    public ReadOnlyArray<InputControlScheme> controlSchemes => asset.controlSchemes;

    public bool Contains(InputAction action)
    {
        return asset.Contains(action);
    }

    public IEnumerator<InputAction> GetEnumerator()
    {
        return asset.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator()
    {
        return GetEnumerator();
    }

    public void Enable()
    {
        asset.Enable();
    }

    public void Disable()
    {
        asset.Disable();
    }

    public IEnumerable<InputBinding> bindings => asset.bindings;

    public InputAction FindAction(string actionNameOrId, bool throwIfNotFound = false)
    {
        return asset.FindAction(actionNameOrId, throwIfNotFound);
    }

    public int FindBinding(InputBinding bindingMask, out InputAction action)
    {
        return asset.FindBinding(bindingMask, out action);
    }

    // Player
    private readonly InputActionMap m_Player;
    private List<IPlayerActions> m_PlayerActionsCallbackInterfaces = new List<IPlayerActions>();
    private readonly InputAction m_Player_Movement;
    private readonly InputAction m_Player_Run;
    private readonly InputAction m_Player_Look;
    private readonly InputAction m_Player_Jump;
    private readonly InputAction m_Player_Squat;
    private readonly InputAction m_Player_Interection;
    private readonly InputAction m_Player_Setting;
    private readonly InputAction m_Player_Simulate;
    private readonly InputAction m_Player_Accept;
    private readonly InputAction m_Player_CancelUI;
    private readonly InputAction m_Player_Zoom;
    public struct PlayerActions
    {
        private @PlayerInputs m_Wrapper;
        public PlayerActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @Movement => m_Wrapper.m_Player_Movement;
        public InputAction @Run => m_Wrapper.m_Player_Run;
        public InputAction @Look => m_Wrapper.m_Player_Look;
        public InputAction @Jump => m_Wrapper.m_Player_Jump;
        public InputAction @Squat => m_Wrapper.m_Player_Squat;
        public InputAction @Interection => m_Wrapper.m_Player_Interection;
        public InputAction @Setting => m_Wrapper.m_Player_Setting;
        public InputAction @Simulate => m_Wrapper.m_Player_Simulate;
        public InputAction @Accept => m_Wrapper.m_Player_Accept;
        public InputAction @CancelUI => m_Wrapper.m_Player_CancelUI;
        public InputAction @Zoom => m_Wrapper.m_Player_Zoom;
        public InputActionMap Get() { return m_Wrapper.m_Player; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(PlayerActions set) { return set.Get(); }
        public void AddCallbacks(IPlayerActions instance)
        {
            if (instance == null || m_Wrapper.m_PlayerActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Add(instance);
            @Movement.started += instance.OnMovement;
            @Movement.performed += instance.OnMovement;
            @Movement.canceled += instance.OnMovement;
            @Run.started += instance.OnRun;
            @Run.performed += instance.OnRun;
            @Run.canceled += instance.OnRun;
            @Look.started += instance.OnLook;
            @Look.performed += instance.OnLook;
            @Look.canceled += instance.OnLook;
            @Jump.started += instance.OnJump;
            @Jump.performed += instance.OnJump;
            @Jump.canceled += instance.OnJump;
            @Squat.started += instance.OnSquat;
            @Squat.performed += instance.OnSquat;
            @Squat.canceled += instance.OnSquat;
            @Interection.started += instance.OnInterection;
            @Interection.performed += instance.OnInterection;
            @Interection.canceled += instance.OnInterection;
            @Setting.started += instance.OnSetting;
            @Setting.performed += instance.OnSetting;
            @Setting.canceled += instance.OnSetting;
            @Simulate.started += instance.OnSimulate;
            @Simulate.performed += instance.OnSimulate;
            @Simulate.canceled += instance.OnSimulate;
            @Accept.started += instance.OnAccept;
            @Accept.performed += instance.OnAccept;
            @Accept.canceled += instance.OnAccept;
            @CancelUI.started += instance.OnCancelUI;
            @CancelUI.performed += instance.OnCancelUI;
            @CancelUI.canceled += instance.OnCancelUI;
            @Zoom.started += instance.OnZoom;
            @Zoom.performed += instance.OnZoom;
            @Zoom.canceled += instance.OnZoom;
        }

        private void UnregisterCallbacks(IPlayerActions instance)
        {
            @Movement.started -= instance.OnMovement;
            @Movement.performed -= instance.OnMovement;
            @Movement.canceled -= instance.OnMovement;
            @Run.started -= instance.OnRun;
            @Run.performed -= instance.OnRun;
            @Run.canceled -= instance.OnRun;
            @Look.started -= instance.OnLook;
            @Look.performed -= instance.OnLook;
            @Look.canceled -= instance.OnLook;
            @Jump.started -= instance.OnJump;
            @Jump.performed -= instance.OnJump;
            @Jump.canceled -= instance.OnJump;
            @Squat.started -= instance.OnSquat;
            @Squat.performed -= instance.OnSquat;
            @Squat.canceled -= instance.OnSquat;
            @Interection.started -= instance.OnInterection;
            @Interection.performed -= instance.OnInterection;
            @Interection.canceled -= instance.OnInterection;
            @Setting.started -= instance.OnSetting;
            @Setting.performed -= instance.OnSetting;
            @Setting.canceled -= instance.OnSetting;
            @Simulate.started -= instance.OnSimulate;
            @Simulate.performed -= instance.OnSimulate;
            @Simulate.canceled -= instance.OnSimulate;
            @Accept.started -= instance.OnAccept;
            @Accept.performed -= instance.OnAccept;
            @Accept.canceled -= instance.OnAccept;
            @CancelUI.started -= instance.OnCancelUI;
            @CancelUI.performed -= instance.OnCancelUI;
            @CancelUI.canceled -= instance.OnCancelUI;
            @Zoom.started -= instance.OnZoom;
            @Zoom.performed -= instance.OnZoom;
            @Zoom.canceled -= instance.OnZoom;
        }

        public void RemoveCallbacks(IPlayerActions instance)
        {
            if (m_Wrapper.m_PlayerActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IPlayerActions instance)
        {
            foreach (var item in m_Wrapper.m_PlayerActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_PlayerActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public PlayerActions @Player => new PlayerActions(this);

    // RhythmMode
    private readonly InputActionMap m_RhythmMode;
    private List<IRhythmModeActions> m_RhythmModeActionsCallbackInterfaces = new List<IRhythmModeActions>();
    private readonly InputAction m_RhythmMode_RhythmInput;
    public struct RhythmModeActions
    {
        private @PlayerInputs m_Wrapper;
        public RhythmModeActions(@PlayerInputs wrapper) { m_Wrapper = wrapper; }
        public InputAction @RhythmInput => m_Wrapper.m_RhythmMode_RhythmInput;
        public InputActionMap Get() { return m_Wrapper.m_RhythmMode; }
        public void Enable() { Get().Enable(); }
        public void Disable() { Get().Disable(); }
        public bool enabled => Get().enabled;
        public static implicit operator InputActionMap(RhythmModeActions set) { return set.Get(); }
        public void AddCallbacks(IRhythmModeActions instance)
        {
            if (instance == null || m_Wrapper.m_RhythmModeActionsCallbackInterfaces.Contains(instance)) return;
            m_Wrapper.m_RhythmModeActionsCallbackInterfaces.Add(instance);
            @RhythmInput.started += instance.OnRhythmInput;
            @RhythmInput.performed += instance.OnRhythmInput;
            @RhythmInput.canceled += instance.OnRhythmInput;
        }

        private void UnregisterCallbacks(IRhythmModeActions instance)
        {
            @RhythmInput.started -= instance.OnRhythmInput;
            @RhythmInput.performed -= instance.OnRhythmInput;
            @RhythmInput.canceled -= instance.OnRhythmInput;
        }

        public void RemoveCallbacks(IRhythmModeActions instance)
        {
            if (m_Wrapper.m_RhythmModeActionsCallbackInterfaces.Remove(instance))
                UnregisterCallbacks(instance);
        }

        public void SetCallbacks(IRhythmModeActions instance)
        {
            foreach (var item in m_Wrapper.m_RhythmModeActionsCallbackInterfaces)
                UnregisterCallbacks(item);
            m_Wrapper.m_RhythmModeActionsCallbackInterfaces.Clear();
            AddCallbacks(instance);
        }
    }
    public RhythmModeActions @RhythmMode => new RhythmModeActions(this);
    public interface IPlayerActions
    {
        void OnMovement(InputAction.CallbackContext context);
        void OnRun(InputAction.CallbackContext context);
        void OnLook(InputAction.CallbackContext context);
        void OnJump(InputAction.CallbackContext context);
        void OnSquat(InputAction.CallbackContext context);
        void OnInterection(InputAction.CallbackContext context);
        void OnSetting(InputAction.CallbackContext context);
        void OnSimulate(InputAction.CallbackContext context);
        void OnAccept(InputAction.CallbackContext context);
        void OnCancelUI(InputAction.CallbackContext context);
        void OnZoom(InputAction.CallbackContext context);
    }
    public interface IRhythmModeActions
    {
        void OnRhythmInput(InputAction.CallbackContext context);
    }
}
