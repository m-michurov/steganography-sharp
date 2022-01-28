## Image LSB encoder

Steganography is the practice of concealing a message 
within another message or a physical object. In 
computing/electronic contexts, a computer file, message,
image, or video is concealed within another file, 
message, image, or video. [[From Wikipedia, the free encyclopedia](https://en.wikipedia.org/wiki/Steganography)]

This project implements least significant bit encoding,
i. e. uses lowest bits of images to conceal arbitrary 
data.

# Usage

CLI project provides a command line interface. You may want to create a 
standalone executable.

```bash
> CLI.exe [ACTION] [OPTIONS]
```

Actions:

    encode     Encode data.
    
    decode     Decode data.
    
    help       Display more information on a specific command.
    
    version    Display version information.

### Encoding data

```bash
> CLI.exe encode [OPTIONS]
```

Options:

    -o, --out       (Default: secret.png) Output file name.
    
    --verify        (Default: false) Decode encoded data and compare it to original data.
    
    --help          Display this help screen.
    
    --version       Display version information.
    
    value pos. 0    Required. Container image path.
    
    value pos. 1    Required. Data to encode.

Example:

```bash
> CLI.exe encode image.jpg cats.gif --out="image with cats.png" --verify
```

### Decoding data

```bash
> CLI.exe decode [OPTIONS]
```

    -o, --out       (Default: data) Output file name.
    
    --help          Display this help screen.
    
    --version       Display version information.
    
    value pos. 0    Required. Container image path.

Example:

```bash
> CLI.exe decode "image with cats.png" --out="probably cats.gif"
```