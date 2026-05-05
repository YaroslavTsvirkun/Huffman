# Huffman

`Huffman` — це C#-бібліотека для кодування та декодування тексту за алгоритмом Хаффмана. Репозиторій містить основну бібліотеку та автоматичні тести.

## Склад репозиторію

- `Huffman Compressor` — основна бібліотека з API для побудови дерева, кодування та декодування.
- `Huffman.Tests` — набір автоматичних xUnit-тестів для основних сценаріїв і крайових випадків.
- `Huffman.slnx` — solution-файл у сучасному форматі `.slnx`.

## Що вміє бібліотека

- будує Huffman-дерево на основі вхідного тексту;
- генерує таблицю кодів для символів;
- кодує рядок у `BitArray`;
- декодує бітову послідовність назад у вихідний текст;
- перевіряє помилки використання API, зокрема порожній ввід, відсутню побудову дерева та некоректну бітову послідовність.

## Вимоги

- .NET SDK 10 або новіший.

Перевірити встановлені SDK:

```powershell
dotnet --list-sdks
```

## Збірка

```powershell
dotnet build .\Huffman.slnx
```

## Запуск тестів

```powershell
dotnet test .\Huffman.slnx
```

## Використання бібліотеки

```csharp
using Huffman.Compressor;

var codec = new HuffmanCodec();
codec.Build("hello world");

var encoded = codec.Encode("hello world");
var decoded = codec.Decode(encoded);

Console.WriteLine(decoded); // hello world
```

## Автоматичні перевірки

У тестовому проєкті вже покриті такі сценарії:

- round-trip для звичайного тексту;
- round-trip для рядка з одного символу;
- помилка на порожньому джерелі;
- помилка під час кодування символів поза поточним деревом;
- помилка на неповній бітовій послідовності;
- помилка при виклику `Encode` до `Build`.

## Структура

```text
Huffman/
|-- Directory.Build.props
|-- LICENCE
|-- README.md
|-- Huffman.slnx
|-- Huffman Compressor/
|   |-- Huffman.Compressor.csproj
|   |-- HuffmanEncryption.cs
|   `-- Node.cs
`-- Huffman.Tests/
    |-- Huffman.Tests.csproj
    `-- HuffmanCodecTests.cs
```

## API-нотатка

Клас `HuffmanEncryption` збережено лише для сумісності зі старішим кодом і позначено як legacy-ім'я. Для нового коду варто використовувати `HuffmanCodec`.
