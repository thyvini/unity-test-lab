namespace FSharp

open UnityEngine

type Generator() =
    inherit MonoBehaviour()

    [<SerializeField>]
    [<DefaultValue>] val mutable private _chunkModels : GameObject[]

    [<DefaultValue>] val mutable private _map : Object[,]
    let size = 100

    let remap value from1 to1 from2 to2 = 
        (value - from1) / (to1 - from1) * (to2 - from2) + from2

    member private this.getFromNoise x z =
        let noisedValue = float (Mathf.PerlinNoise(float32 ((float x) * 10.0 / float size), float32((float z) * 10.0 / float size)))
        remap noisedValue 0.0 1.0 0.0 (float this._chunkModels.Length)
        |> floor
        |> int

    
    member this.Awake() =
        this._map <- Array2D.init 100 100 (fun i j ->
            GameObject.Instantiate (this._chunkModels.[this.getFromNoise i j], (Vector3 (float32 i, 0.0f, float32 j)), Quaternion.identity))