using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour {
        public static AudioManager Instance;
		[SerializeField] private AudioMixer masterAudio;
        [SerializeField] private AudioSource musicSource;
        [SerializeField] private List<AudioSource> SFXSource = new List<AudioSource>();
        [SerializeField] private AudioClip[] sound;
        [SerializeField] private AudioClip[] music;

        private void Awake() {
			if(Instance != null) {
				Destroy(this);
                return;
			}

			DontDestroyOnLoad(this);
			Instance = this;

        }


        public void PlaySound(string _name, float _volume = 1, float _pitch = 1f) {
            AudioSource _audioSource = GetFirstAvailableAudioSource();
            
            AudioClip _clip = null;
            foreach (AudioClip clip in sound) {
                if (clip.name == _name) {
                    _clip = clip;
                }
            }

            if (!_clip) {
                return;
            }
            
            _audioSource.clip = _clip;
            _audioSource.volume = _volume;
            _audioSource.pitch = _pitch;
            _audioSource.Play();
        }

        /*public void StopSound() {
            if (SFXSource) {
                SFXSource.Stop();
            }
        }*/

        bool _dontReplayCheck;
        
        public void PlayMusic(string _name) {
            if (musicSource) {
                AudioClip _clip = null;
                foreach (AudioClip clip in music) {
                    if (clip.name == _name) {
                        _clip = clip;
                    }
                }

                if (_clip != null) {
                    musicSource.clip = _clip;
                    musicSource.loop = _name == "GMTK_Main_Soundtrack";
                    if (musicSource.loop)
                    {
                        if (_dontReplayCheck)
                            return;
                        _dontReplayCheck = true;
                    }
                    else
                    {
                        _dontReplayCheck = false;
                    }
                    musicSource.Stop();
                    musicSource.Play();
                }
            }
        }

        private AudioSource GetFirstAvailableAudioSource() {
            foreach (AudioSource _source in SFXSource) {
                if (!_source.isPlaying) {
                    return _source;
                }
            }

            GameObject _go = Instantiate(SFXSource[0].gameObject, transform);
            AudioSource _newSource = _go.GetComponent<AudioSource>();
            SFXSource.Add(_newSource);
            return _newSource;
        }
        
        /*private void Start() {
            (PlayerPrefs.GetFloat("SavedMasterVolume", 100), false); //Changing this will also set the volume because of the OnValueChanged
        }

        public void ToggleSound() {
            float _value = PlayerPrefs.GetFloat("SavedMasterVolume", 100);

            if (_value > 1) {
                tempMasterVolume = soundSlider.value;
                SetVolume(0);
            } else {
                SetVolume(tempMasterVolume);
            }

        }

        public void SetVolume(float _value, bool playSound = true) {
            if (_value < 1) {
                _value = .001f;
            }

            if (_value > 66f) {
                soundIcon.sprite = soundHigh;
            } else if (_value > 33f) {
                soundIcon.sprite = soundMedium;
            } else if (_value > 1f) {
                soundIcon.sprite = soundLow;
            } else {
                soundIcon.sprite = soundOff;
            }

            if (playSound) {
                audioSample.Play();
            }

            RefreshSlider(_value);
            PlayerPrefs.SetFloat("SavedMasterVolume", _value);
            masterAudio.SetFloat("MasterVolume", Mathf.Log10(_value / 100) * 20f);
        }

        public void SetVolumeFromSlider(bool _playSound = false) {
            SetVolume(soundSlider.value, _playSound);
        }

        public void RefreshSlider(float _value) {
            soundSlider.value = _value;
        }*/
}
