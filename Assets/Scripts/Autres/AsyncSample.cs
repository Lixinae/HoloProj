using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Threading;
/*using System.Threading.Tasks;
using System;
using UnityEngine.WSA;
using UnityEngine.UI;

#if ENABLE_WINMD_SUPPORT
using Windows.Storage;
using Windows.Storage.Streams;
using Windows.Storage.Pickers;
#endif

public class AsyncSample : MonoBehaviour {
    public Text label;
    public GameObject cube;

#if ENABLE_WINMD_SUPPORT 
    private FileOpenPicker openPicker;
#endif

    void Start() {
        UnityEngine.Debug.LogFormat("UnityThread: {0}", Thread.CurrentThread.ManagedThreadId);

#if ENABLE_WINMD_SUPPORT
        UnityEngine.WSA.Application.InvokeOnUIThread(OpenFileAsync, false);
#else
        UnityEngine.Debug.Log( "ENABLE_WINMD_SUPPORT false" ); 
#endif
    }

    void ThreadCallback() {
        UnityEngine.Debug.LogFormat("ThreadCallback() on thread: \t{0}", Thread.CurrentThread.ManagedThreadId);

        cube.transform.Rotate(Vector3.forward, 25f);
    }

#if ENABLE_WINMD_SUPPORT
    public async void OpenFileAsync() {
        UnityEngine.Debug.LogFormat("OpenFileAsync() on Thread: {0}", Thread.CurrentThread.ManagedThreadId);

        openPicker = new FileOpenPicker();

        //openPicker.ViewMode = PickerViewMode.Thumbnail;
        //openPicker.SuggestedStartLocation = PickerLocationId.Objects3D;
        //openPicker.FileTypeFilter.Add(".fbx");
        openPicker.FileTypeFilter.Add("*");

        StorageFile file = await openPicker.PickSingleFileAsync();
        string labelText = String.Empty;
        if (file != null) {
            // Application now has read/write access to the picked file 
            labelText = "Picked file: " + file.DisplayName;
        }
        else {
            // The picker was dismissed with no selected file 
            labelText = "File picker operation cancelled";
        }
        UnityEngine.Debug.Log(labelText);
        UnityEngine.WSA.Application.InvokeOnAppThread(ThreadCallback, false);
        UnityEngine.WSA.Application.InvokeOnAppThread(new AppCallbackItem(() => { label.text = labelText; }), false);
    }
}
#endif
*/