using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Auth;
using Firebase.Database;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using MirrorBasics;

public class FirebaseManager : MonoBehaviour
    {
    public string username;
        //Firebase variables
        [Header("Firebase")]
        public DependencyStatus dependencyStatus;
        public FirebaseAuth auth;
        public FirebaseUser User;
        public DatabaseReference DBreference;

        //Login variables
        [Header("Login")]
        public InputField emailLoginField;
        public InputField passwordLoginField;
        public Text warningLoginText;
        public Text confirmLoginText;

        //Register variables
        [Header("Register")]
        public InputField usernameRegisterField;
        public InputField emailRegisterField;
        public InputField passwordRegisterField;
        public InputField passwordRegisterVerifyField;
        public Text warningRegisterText;

        void Awake()
        {
            //Check that all of the necessary dependencies for Firebase are present on the system
            FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
            {
                dependencyStatus = task.Result;
                if (dependencyStatus == DependencyStatus.Available)
                {
                //If they are avalible Initialize Firebase
                InitializeFirebase();
                }
                else
                {
                    Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
                }
            });
        }

        private void InitializeFirebase()
        {
            Debug.Log("Setting up Firebase Auth");
            //Set the authentication instance object
            auth = FirebaseAuth.DefaultInstance;
            DBreference = FirebaseDatabase.DefaultInstance.RootReference;
            
            
    }

        #region BUTTONS
        //Function for the login button
        public void LoginButton()
        {
            //Call the login coroutine passing the email and password
            StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
        }
        //Function for the register button
        public void RegisterButton()
        {
            //Call the register coroutine passing the email, password, and username
            StartCoroutine(Register(emailRegisterField.text, passwordRegisterField.text, usernameRegisterField.text));
        }

    //Function for the save button

    #endregion

    public void SaveData(StoreData data)
    {
        Debug.Log(username);
        StartCoroutine(UpdateUsernameAuth(username));
        StartCoroutine(UpdateUsernameDatabase(username));

        //StartCoroutine(FUpdateRole(data.GetRoleIntData()));


    }

    #region Register & Login METHODS
    private IEnumerator Login(string _email, string _password)
        {
            //Call the Firebase auth signin function passing the email and password
            var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

            if (LoginTask.Exception != null)
            {
                //If there are errors handle them
                Debug.LogWarning(message: $"Failed to register task with {LoginTask.Exception}");
                FirebaseException firebaseEx = LoginTask.Exception.GetBaseException() as FirebaseException;
                AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                string message = "Login Failed!";
                switch (errorCode)
                {
                    case AuthError.MissingEmail:
                        message = "Missing Email";
                        break;
                    case AuthError.MissingPassword:
                        message = "Missing Password";
                        break;
                    case AuthError.WrongPassword:
                        message = "Wrong Password";
                        break;
                    case AuthError.InvalidEmail:
                        message = "Invalid Email";
                        break;
                    case AuthError.UserNotFound:
                        message = "Account does not exist";
                        break;
                }
                warningLoginText.text = message;
            }
            else
            {
                //User is now logged in
                //Now get the result
                User = LoginTask.Result;
                Debug.LogFormat("User signed in successfully: {0} ({1})", User.DisplayName, User.Email);
                warningLoginText.text = "";
                confirmLoginText.text = "Logged In";
                

                yield return new WaitForSeconds(2.0f);
                username = User.DisplayName;
            
                DontDestroyOnLoad(this);


                SceneManager.LoadScene(1, LoadSceneMode.Single);

            }

            
        }

        private IEnumerator Register(string _email, string _password, string _username)
        {
            if (_username == "")
            {
                //If the username field is blank show a warning
                warningRegisterText.text = "Missing Username";
            }
            else if (passwordRegisterField.text != passwordRegisterVerifyField.text)
            {
                //If the password does not match show a warning
                warningRegisterText.text = "Password Does Not Match!";
            }
            else
            {
                //Call the Firebase auth signin function passing the email and password
                var RegisterTask = auth.CreateUserWithEmailAndPasswordAsync(_email, _password);
                //Wait until the task completes
                yield return new WaitUntil(predicate: () => RegisterTask.IsCompleted);

                if (RegisterTask.Exception != null)
                {
                    //If there are errors handle them
                    Debug.LogWarning(message: $"Failed to register task with {RegisterTask.Exception}");
                    FirebaseException firebaseEx = RegisterTask.Exception.GetBaseException() as FirebaseException;
                    AuthError errorCode = (AuthError)firebaseEx.ErrorCode;

                    string message = "Register Failed!";
                    switch (errorCode)
                    {
                        case AuthError.MissingEmail:
                            message = "Missing Email";
                            break;
                        case AuthError.MissingPassword:
                            message = "Missing Password";
                            break;
                        case AuthError.WeakPassword:
                            message = "Weak Password";
                            break;
                        case AuthError.EmailAlreadyInUse:
                            message = "Email Already In Use";
                            break;
                    }
                    warningRegisterText.text = message;
                }


                else
                {
                    //User has now been created
                    //Now get the result
                    User = RegisterTask.Result;

                    if (User != null)
                    {
                        //Create a user profile and set the username
                        UserProfile profile = new UserProfile { DisplayName = _username };

                        //Call the Firebase auth update user profile function passing the profile with the username
                        var ProfileTask = User.UpdateUserProfileAsync(profile);
                        //Wait until the task completes
                        yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

                        if (ProfileTask.Exception != null)
                        {
                            //If there are errors handle them
                            Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
                            FirebaseException firebaseEx = ProfileTask.Exception.GetBaseException() as FirebaseException;
                            AuthError errorCode = (AuthError)firebaseEx.ErrorCode;
                            warningRegisterText.text = "Username Set Failed!";
                        }
                        else
                        {
                            //Username is now set
                            //Now return to login screen
                            
                            //UILoginManager.instance.LoginUI();
                            warningRegisterText.text = "";

                        // ------ AUTO LOGIN
                        // Login(_email, _password);

                        var LoginTask = auth.SignInWithEmailAndPasswordAsync(_email, _password);
                        //Wait until the task completes
                        yield return new WaitUntil(predicate: () => LoginTask.IsCompleted);

                        username = User.DisplayName;

                        DontDestroyOnLoad(this);


                        SceneManager.LoadScene(1, LoadSceneMode.Single);
                    }
                    }
                }
            }
        }
        #endregion

        #region UPDATE METHODS
        private IEnumerator UpdateUsernameAuth(string _username)
        {
            //Create a user profile and set the username
            UserProfile profile = new UserProfile { DisplayName = _username };

            //Call the Firebase auth update user profile function passing the profile with the username
            var ProfileTask = User.UpdateUserProfileAsync(profile);
            //Wait until the task completes
            yield return new WaitUntil(predicate: () => ProfileTask.IsCompleted);

            if (ProfileTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {ProfileTask.Exception}");
            }
            else
            {
             //Debug.Log("Success Update username auth");
            }
        }
        private IEnumerator UpdateUsernameDatabase(string _username)
        {
            //Set the currently logged in user username in the database
            var DBTask = DBreference.Child("users").Child(User.UserId).Child("username").SetValueAsync(_username);

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else
            {
             //Debug.Log("Success Update Username database");
            }
        }

        public IEnumerator UpdateMatches(string json)
        {
            TurnManager tn = FindObjectOfType<TurnManager>();
        //Set the currently logged in user xp

            var DBTask = DBreference.Child("users").Child(User.UserId).Child("username").Child(username).SetRawJsonValueAsync(json);  //SetValueAsync(_role);

            yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

            if (DBTask.Exception != null)
            {
                Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            }
            else
            {
                Debug.LogWarning("Salvataggio completato");
            }
        }
    #endregion

    public IEnumerator LoadUserData(StoreData data)
    {
        Debug.Log(DBreference);
        Debug.Log("Start coroutine");
        //Get the currently logged in user data
        var DBTask = DBreference.Child("users").Child(User.UserId).Child("username").Child(username).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
        }
        else if (DBTask.Result.Value == null)
        {
            data.role = 0;
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            int result = System.Int16.Parse(snapshot.Child("role").Value.ToString());
            data.role = (StoreData.TextRole)result;


        }
    }

    public IEnumerator LoadMatchData(TurnManager tm)
    {
        var DBTask = DBreference.Child("users").Child(User.UserId).GetValueAsync();

        yield return new WaitUntil(predicate: () => DBTask.IsCompleted);

        if (DBTask.Exception != null)
        {
            Debug.LogWarning(message: $"Failed to register task with {DBTask.Exception}");
            tm.LoaderFlag = true;
        }
        else if (DBTask.Result.Value == null)
        {
            tm.JsonLoader = null;
            tm.LoaderFlag = true;
        }
        else
        {
            //Data has been retrieved
            DataSnapshot snapshot = DBTask.Result;

            tm.JsonLoader = snapshot.Child("users").Child(User.UserId).Value.ToString();
            tm.LoaderFlag = true;
            


        }

    }
}