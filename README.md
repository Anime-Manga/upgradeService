## 💽Upgrade Service
Questo progetto verrà utilizzato per scaricare i nuovi episodi
### Information general:
> Note: `not` require volume mounted on Docker

### Dependencies
| Services | Required |
| ------ | ------ |
| Api | ✅  |
| RabbitMQ | ✅  |
| Notify | ⛔ |

### Variabili globali richiesti:
```sh
example:
    #--- rabbit ---
    USERNAME_RABBIT: "guest" #guest [default]
    PASSWORD_RABBIT: "guest" #guest [default]
    ADDRESS_RABBIT: "localhost" #localhost [default]
    
    #--- API ---
    ADDRESS_API: "localhost" #localhost [default]
    PORT_API: "33333" #5000 [default]
    PROTOCOL_API: "http" or "https" #http [default]
    
    #--- Logger ---
    LOG_LEVEL: "Debug|Info|Error" #Info [default]
    WEBHOOK_DISCORD_DEBUG: "url" [not require]
    
    #--- General ---
    BASE_PATH: "/folder/anime" or "D:\\\\Directory\Anime" #http [default]
    TIME_REFRESH: "60000" <-- milliseconds #1200000 [default] 20 minutes
    LIMIT_THREAD_PARALLEL: "8" #5 [default]
    SELECT_SERVICE: "book or video" #video
```