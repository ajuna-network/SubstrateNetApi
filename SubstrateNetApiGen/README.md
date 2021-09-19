# SubstrateNetApi Extension Template

This is a simple template for custom extension that integrate a pallet or set of pallets functionality for [SubstrateNetApi](https://github.com/JetonNetwork/SubstrateNetApi).

## Call

Metadata of the extrinswic to implement
```json
        "Calls":[
            {
               "Name":"do_something",
               "Arguments":[
                  {
                     "Name":"something",
                     "Type":"u32"
                  }
               ],
               "Documentations":[
                  " An example dispatchable that takes a singles value as a parameter, writes the value to",
                  " storage and emits an event. This function must be dispatched by a signed extrinsic."
               ]
            },
```
Implementation
```csharp
        public static GenericExtrinsicCall DoSomething(U32 something)
        {
            return new GenericExtrinsicCall("TemplateModule", "do_something", something);
        }
```

## Type

Types are grouped into base types, enum types and struct types, implement the special types your using within the api. For reference implementation check the [ConnectFour](https://github.com/JetonNetwork/JtonConnectFourExt). 
```csharp
    #region BASE_TYPES

    #endregion

    #region ENUM_TYPES

    #endregion

    #region STRUCT_TYPES

    #endregion
```

## Test

updated your wesocket url
```csharp
        private const string WebSocketUrl = "ws://127.0.0.1:9944";
```

Add special type converters to the client
```csharp
        [SetUp]
        public void Setup()
        {
            ...
            ...
            ...
            _substrateClient = new SubstrateClient(new Uri(WebSocketUrl));
            // Add your types here that you're using
            _substrateClient.RegisterTypeConverter(new GenericTypeConverter<SpecialTypeForThisExt>());
        }
```

Tests start with connecting and closing in between do your tests
```csharp
        [Test]
        public async Task GetMethodChainNameTestAsync()
        {
            await _substrateClient.ConnectAsync();

            var result = await _substrateClient.GetMethodAsync<string>("system_chain");
            Assert.AreEqual("Development", result);

            await _substrateClient.CloseAsync();
        }
```
