using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AATManager : MonoBehaviour
{
	public int sampleDataLength = 1024;
	[Tooltip("Pointer to the player object")]
	public Transform player;
	[Tooltip("Sounds beyond this distance are not represented")]
	public float maxDistance = 50f;
	[Tooltip("Number of frames between each cycle")]
	public float updateRate = 5f;

	// Tag color configuration
	[System.Serializable]
	public struct AudioTags { public string tagName; public Color tagColor; }
	[Header("Tags color configuration")]
	public AudioTags[] audioType;

	// Sphere configuration parameters
	[Header("Sphere parameters")]
	[Tooltip("Enable/Disable sphere tool")]
	public bool enableSphere;
	[Tooltip("Pointer to the physical sphere object")]
	public GameObject sphere;
	[Tooltip("Strength of the springs that pull the sphere's spikes")]
	public float springForce = 40f; 
	[Tooltip("Sphere's resistance to forces")]
	public float damping = 5f;
	[Tooltip("Strength of the force that pulls the spikes")]
	public float force = 40f;
	[Tooltip("The rate at which de force decays with distance, making wider or narrower spikes")]
	public float forceDecay = 0.5f;
	[Tooltip("Radius of the sphere object")]
	public float radius = 1f;
	[Tooltip("Default color of the sphere")]
	public Color sphereColor;
	[Tooltip("Minimum force applied by a sound")]
	public float offset = 0.01f;

	// Compass configuration parameters
	[Header("Compass parameters")]
	[Tooltip("Enable/Disable compass tool")]
	public bool enableCompass;
	[Tooltip("Pointer to the game main camera")]
	public Camera mainCamera;
	[Tooltip("Icon to represent sounds in the compass")]
	public Sprite icon;
	[Tooltip("Arrow icon for objects out of the camera view")]
	public Sprite arrow; 
	[Tooltip("Prefab for sound icons in the compass")]
	public GameObject compassMarker;
	[Tooltip("Foreground compass image, usually the cardinal points")]
	public RawImage compassImage;
	[Tooltip("Size of the sound markers")]
	public int markerSize = 2;

	// Whenever an AATSource is created, it is added to this list
	List<AATSource> sources = new List<AATSource>();

	float compassUnit;
	float[] clipSampleData;

	Vector3[] originalVertices, displacedVertices;
	Vector3[] vertexVelocities;
	Color[] colors, originalColors;

	Mesh deformingMesh;

	void Start()
	{
		clipSampleData = new float[sampleDataLength];

		// Sphere initialization
		if (enableSphere)
		{
			deformingMesh = sphere.GetComponent<MeshFilter>().mesh;
			originalVertices = deformingMesh.vertices;
			displacedVertices = new Vector3[originalVertices.Length];
			vertexVelocities = new Vector3[originalVertices.Length];
			colors = new Color[originalVertices.Length];
			originalColors = new Color[originalVertices.Length];

			for (int i = 0; i < originalVertices.Length; i++)
			{
				colors[i] = sphereColor;
				originalColors[i] = sphereColor;
			}

			deformingMesh.colors = colors;

			for (int i = 0; i < originalVertices.Length; i++)
				displacedVertices[i] = originalVertices[i];
		}

		// Compass initialization
		if (enableCompass)
			compassUnit = compassImage.rectTransform.rect.width / 360f;
	}

	void Update()
	{
		compassImage.uvRect = new Rect(player.localEulerAngles.y / 360f, 0f, 1f, 1f);
		if (Time.frameCount % updateRate == 0)
		{
			foreach (AATSource source in sources)
			{
				if (Vector3.Distance(player.transform.position, source.positionXYZ) < maxDistance)
				{
					if (enableSphere)
						AddForceToSphere(source);

					if (enableCompass)
						UpdateCompass(source);
				} else
                {
					source.ShowArrow(false, false);
					source.GetIcon().rectTransform.localScale = Vector3.zero;
				}
			}

			if (enableSphere)
				UpdateSphere();
		}
	}

	void AddForceToSphere(AATSource source)
    {
		AudioSource audioSource = source.GetAudioSource();
		if (audioSource.isPlaying)
		{
			// Apply a force pulling sphere vertices around 'point' in the direction of the source
			float soundIntensity = GetSoundIntensity(audioSource) + offset;
			Vector3 direction = sphere.transform.position - source.transform.position;
			Vector3 point = sphere.transform.position - direction.normalized * radius;
			AddDeformingForce(point, source.transform.position, force * soundIntensity, audioSource);
		}
	}

	void UpdateSphere()
    {
		sphere.transform.rotation = Quaternion.identity;

		// Update vertices
		for (int i = 0; i < displacedVertices.Length; i++)
			UpdateVertex(i);

		// Apply modifications to the mesh
		deformingMesh.colors = colors;
		deformingMesh.vertices = displacedVertices;
		deformingMesh.RecalculateNormals();
	}

	void UpdateCompass(AATSource source)
	{
		// Hide marker preventively
		source.ShowArrow(false, false);
		source.GetIcon().rectTransform.localScale = Vector3.zero;

		AudioSource audioSource = source.GetAudioSource();
		if (audioSource.isPlaying)
		{
			Vector2 posInCompass = GetPosOnCompass(source);
			float soundIntensity = GetSoundIntensity(audioSource);

			if (soundIntensity > 0)
			{
				// Place icon
				source.GetIcon().rectTransform.anchoredPosition = posInCompass;
				float markerScale = Normalize(soundIntensity, 0, 1, 0, markerSize);
				source.GetIcon().rectTransform.localScale = Vector3.one * Mathf.Sqrt(markerScale) * 2;

				// Place arrow
				posInCompass.x += 10; posInCompass.y -= 10;
				source.GetArrow().rectTransform.anchoredPosition = posInCompass;

				Vector3 screenPoint = mainCamera.WorldToViewportPoint(source.gameObject.transform.position);
				// Only show arrows for sounds in front of the player
				if (screenPoint.x > 0 && screenPoint.x < 1 && screenPoint.z > 0)
				{
					// Sound is above the player, show arrow
					if (screenPoint.y > 1)
						source.ShowArrow(true, false);
					// Sound is below the player, show arrow flipped
					else if (screenPoint.y < 0)
						source.ShowArrow(true, true);
				}
			}
		}
	}

	// Deform a vertex according to the forces applied to it and change vertex color
	void UpdateVertex(int i)
	{
		// Vertex deformation
		Vector3 velocity = vertexVelocities[i];
		Vector3 displacement = displacedVertices[i] - originalVertices[i];
		displacement *= forceDecay;
		velocity -= displacement * springForce * Time.deltaTime * updateRate;
		velocity *= 1f - damping * Time.deltaTime * updateRate;

		vertexVelocities[i] = velocity;
		displacedVertices[i] += velocity * (Time.deltaTime / forceDecay) * updateRate;

		// Vertex color
		float maxvalue = 0.25f;
		if (displacement.magnitude <= 0.1)
			colors[i] = sphereColor;
		else
		{
			float difference; //Between 0 and 1. 0 strip to the color of the vertex, 1 strip to the color of the sphere

			if (displacement.magnitude > maxvalue)
				difference = 0f;
			else
				difference = 1 - (displacement.magnitude / (maxvalue - 0.1f));

			colors[i] = Color.Lerp(colors[i], sphereColor, difference);
		}
	}

	// Apply a force to each vertex of the mesh
	public void AddDeformingForce(Vector3 point, Vector3 target, float force, AudioSource source)
	{
		point = sphere.transform.InverseTransformPoint(point);
		for (int i = 0; i < displacedVertices.Length; i++)
			AddForceToVertex(i, point, target, force, source);
	}

	// Apply a force to a single vertex of the mesh,
	// based on its distance to the force's origin
	void AddForceToVertex(int i, Vector3 point, Vector3 target, float force, AudioSource source)
	{
		Vector3 pointToVertex = displacedVertices[i] - point;
		pointToVertex *= forceDecay;

		float attenuatedForce = force / Mathf.Pow(pointToVertex.magnitude, 2);
		float velocity = attenuatedForce * Time.deltaTime;

		Vector3 targetToVertex = sphere.transform.position - target;
		vertexVelocities[i] -= targetToVertex.normalized * velocity;

		if (velocity > 0.1)
		{
			if (source.tag == "Untagged")
				colors[i] = sphereColor;
			else
				colors[i] = GetColorByTag(source.tag);
		}
	}

	// Get the intensity of a sound, based on its loudness and distance, in the range [0,1]
	float GetSoundIntensity(AudioSource source)
	{
		source.clip.GetData(clipSampleData, source.timeSamples);
		float clipLoudness = 0f;

		foreach (var sample in clipSampleData)
			clipLoudness += Mathf.Abs(sample);

		clipLoudness /= sampleDataLength;

		float distance = Vector3.Distance(sphere.transform.position, source.gameObject.transform.position);
		float incFactor = distance >= radius ? 1 / distance : 0;

		float soundIntensity = clipLoudness * incFactor * 10 > 1 ? 1 : clipLoudness * incFactor * 10;

		if (soundIntensity > 0.5f) soundIntensity = 0.5f;
		soundIntensity *= 2;
		return soundIntensity;
	}

	// Add a new AATSource to the list and initialize it
	public void AddSource(AATSource source)
	{
		if (enableCompass)
		{
			GameObject newMarker = Instantiate(compassMarker, compassImage.transform);
			source.SetMarker(newMarker);
			source.SetIcon(newMarker.transform.GetChild(0).GetComponent<Image>());
			source.GetIcon().sprite = icon;
			source.GetIcon().color = GetColorByTag(source.gameObject.tag);
			source.SetArrow(newMarker.transform.GetChild(1).GetComponent<Image>());
			source.GetArrow().sprite = arrow;
			source.ShowArrow(false, false);
			source.GetIcon().rectTransform.localScale = Vector3.zero;
		}

		sources.Add(source);
	}

	// Remove an AATSource from the list. Called in OnDestroy()
	public void RemoveSource(AATSource source)
	{
		sources.Remove(source);
	}

	// Translate source position to marker position in the compass
	Vector2 GetPosOnCompass(AATSource source)
	{
		Vector2 playerPos = new Vector2(player.transform.position.x, player.transform.position.z);
		Vector2 playerFwd = new Vector2(player.transform.forward.x, player.transform.forward.z);
		float angle = Vector2.SignedAngle(source.position - playerPos, playerFwd) * 4;

		if (angle > 180) angle = 180;
		if (angle < -180) angle = -180;

		return new Vector2(compassUnit * angle, 0f);
	}

	// Return the color assigned to a given tag
	public Color GetColorByTag(string tag)
    {
        for (int i = 0; i < audioType.Length; i++)
            if (tag == audioType[i].tagName)
                return audioType[i].tagColor;

        return Color.white;
    }

	// Transform range to a new given range
	float Normalize(float val, float valmin, float valmax, float min, float max)
	{
		return (((val - valmin) / (valmax - valmin)) * (max - min)) + min;
	}
}
