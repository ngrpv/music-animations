# Проект
Библиотека-приложение для генерации красивых картинок, основанных на звуковом ряде (и не только). Состоит из нескольких слоёв, в данный момент рабочей точкой входа является VideoGenerator. KernelTests содержит тесты. 
Kernel.Domain содержит билдер, Domain и Services. Сервисы являются воспомогательными и содержат в себе специфичный функционал вроде чтения с диска. 
В Domain находятся отрисовываемые картинки, а также методы для их комбинации, хранятся соответствующие настройки отрисовки.

# Зачем
Проект создан для того, чтобы генерировать картинки по треку. Это может быть использовано, например, в клубах для трансляции с проектора, для домашних кинотеатров и генерации клипов. 

# Точки расширения
Любые возможные средства просмотра. От отрисовки видео в файл и GUI до трансляции на сайт или стрим на твиче. Возможность добавить ещё алгоритмов генерации картинок. Возможность рендера как на GPU, так и на CPU. Возможность real-time работы как плагин. 
Место сборки -- KernelBuilder.
