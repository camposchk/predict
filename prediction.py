from tensorflow.keras.preprocessing import image
from tensorflow.keras import models
import numpy as np
import os

model_path = 'checkpoints/model73.keras'

img_dir = 'test/'

predicted_word = ""

class_names  = { 
        0: '0',
        1: '1',
        2: '2',
        3: '3',
        4: '4',
        5: '5',
        6: '6',
        7: '7',
        8: '8',
        9: '9',
        10: 'A',
        11: 'B',
        12: 'C',
        13: 'D',
        14: 'E',
        15: 'F',
        16: 'G',
        17: 'H',
        18: 'I',
        19: 'J',
        20: 'K',
        21: 'L',
        22: 'M',
        23: 'N',
        24: 'O',
        25: 'P',
        26: 'Q',
        27: 'R',
        28: 'S',
        29: 'T',
        30: 'U',
        31: 'V',
        32: 'W',
        33: 'X',
        34: 'Y',
        35: 'Z',
        36: 'a',
        37: 'b',
        38: 'c',
        39: 'd',
        40: 'e',
        41: 'f',
        42: 'g',
        43: 'h',
        44: 'i',
        45: 'j',
        46: 'k',
        47: 'l',
        48: 'm',
        49: 'n',
        50: 'o',
        51: 'p',
        52: 'q',
        53: 'r',
        54: 's',
        55: 't',
        56: 'u',
        57: 'v',
        58: 'w',
        59: 'x',
        60: 'y',
        61: 'z'
    }


for filename in os.listdir(img_dir):
    if filename.startswith('img_cropped') and filename.endswith('.png'):
        image_path = os.path.join(img_dir, filename)
        img = image.load_img(image_path, target_size=(128, 128))  
        img_array = image.img_to_array(img)
        img_array = np.expand_dims(img_array, axis=0) 

        model = models.load_model(model_path)
        predictions = model.predict(img_array)

        predicted_class_index = np.argmax(predictions)

        predicted_class_name = class_names[predicted_class_index]

        predicted_probability = predictions[0][predicted_class_index]
        
        predicted_word += predicted_class_name

# print("Classe prevista:", predicted_class_name)
# print("Probabilidade:", predicted_probability)

print("Palavra prevista:", predicted_word)

# print(predictions)
