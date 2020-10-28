# Homeworks
Homework1 - Изучить Индексаторы и интерфейсы IEnumerable и IEnumerator. Реализовать консольное приложение в VS Code: Описать собственный  класс, реализующий IEnumerable, IEnumerator который  является коллекцией элементов определенного(на ваш выбор) типа, и перебирается в foreach.
Тип должен так же  содержать индексатор.

Homework2 - Реализовать три сервиса ProductService, ImageService, PriceService, c ProductController, ImageController, PriceController соответственно. Описать модели Product, Image, Price. В контроллерах реализовать методы GET GetAll с корневым роутингом ( GET product/, ...). В моделе Product должны быть поля IEnumerable<Image> Images, IEnumerable<Price> Prices, которые должны заполняться данными из соответствующих сервисов. В качестве хранилища используем любую списочный тип с предзаролненными сущностями.
