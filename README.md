# Strawberry
Simple mobile sequencer with harmonizer

Description of methods:
Sample
1. PlaySound(volume: int, pitch: Pitch, duration: int, pan: int) : void - проигрывает сэмпл с установленными параметрами громкости, высоты, длительности и паранамирования
2. LoadSounds() : void - подгружает звуки определенного сэмпла разных высот
Note
1. PlayNote(sample: Sample) : void - метод для передачи параметров в метод проигрывания сэмпла, то есть функция проигрывания ноты определенным сэмплом
Track
1. Mute() : void - заглушить дорожку проекта
2. Unmute() : void - отменить глушение дорожки проекта
3. SetVolume(volume: int) : void - установить значение громкости для дорожки проекта
4. SetPan(pan: int) : void - установить значение паранамирования для дорожки проекта
5. PlayNotesAtPosition(position: int) : void - проигрывает все ноты дорожки, найденные в определенный момент времени проигрывания дорожки
6. AddNote(note: Note) : void - добавить ноту в контейнер для хранения нот дорожки проекта
7. RemoveNote(note: Note) : void - удалить ноту из контейнера для хранения нот дорожки проекта
Harmonizer
1. Harmonize(notes: Dictionary<int, List<Note>>, project: Project, key: Pitch, chordsPerBar: int) : void - метод анализа, гармонизации дорожки с нотами. эта функция должна также создать в проекте новую дорожку с прописанными аккордами, подобранными по мелодии анализируемой дорожки
Project
1. AddTrack(track: Track) : void - добавить дорожку в проект
2. RemoveTrack(track: Track) : void - удалить дорожку из проекта
3. PlayFromPosition(position: int) : void - метод проигрывания всех дорожек проекта в определенный момент времени проигрывания всего проекта
4. SetBpm(bpm: int) : void - установить темп проигрывания проекта
5. SetTrackPan(track: Track, pan: int) : void - установить параметры паранамирования дорожки
6. SetTrackVolume(track: Track, volume: int) : void - установить параметр громкости дорожки
7. MuteTrack(track: Track) : void - заглушить дорожку проекта
8. UnmuteTrack(track: Track) : void - отменить глушение дорожки проекта
Sequencer
1. SaveCurrentProject(name: string) : void - сохранить текущий проект в базу данных, задав его название
2. LoadProject(name: string) : void - выгрузить проект из базы данных по названию
3. UpdateProjectNames() : void - обновить список названий существующих проектов
4. DeleteProject(name: string) : void - удалить проект из базы данных по названию
