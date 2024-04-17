# import requests

# def corrigir_ortografia(frase):
#     url = 'https://api.languagetool.org/v2/check'
#     payload = {
#         'text': frase,
#         'language': 'pt-BR',  
#     }
#     proxies = {
#         'http': 'https://disrct:etsds10240305@rb-proxy-ca1.bosch.com',
#         'https': 'https://disrct:etsds10240305@rb-proxy-ca1.bosch.com'
#     }
#     response = requests.post(url, data=payload, proxies=proxies)
#     data = response.json()
#     correcoes = []
#     for match in data['matches']:
#         correcoes.append({
#             'mensagem': match['message'],
#             'correcao': match['replacements'][0] if match['replacements'] else '',
#             'posicao_inicio': match['offset'],
#             'posicao_fim': match['offset'] + match['length']
#         })
#     return correcoes


from flask import Flask, request

from tensorflow.keras.preprocessing import image
from tensorflow.keras import models
import numpy as np
import os
import cv2

app = Flask(__name__)

model_path = 'checkpoints/model95.keras'

model = models.load_model(model_path)

def flood_fill_segmentation(image, seed_point):
    segmented_img = image.copy()
    mask = np.zeros((image.shape[0] + 2, image.shape[1] + 2), np.uint8)
    new_val = (255, 255, 255)  
    lo_diff = (17, 17, 17)  
    hi_diff = (17, 17, 17)  
    cv2.floodFill(segmented_img, mask, seed_point, new_val, lo_diff, hi_diff)
    return segmented_img

def find_contours(image):
    gray = cv2.cvtColor(image, cv2.COLOR_BGR2GRAY)
    thresh = cv2.adaptiveThreshold(gray, 255, cv2.ADAPTIVE_THRESH_MEAN_C, cv2.THRESH_BINARY_INV, 11, 2)
    contours, _ = cv2.findContours(thresh, cv2.RETR_EXTERNAL, cv2.CHAIN_APPROX_SIMPLE)
    return contours

def draw_rectangles(contour_img, contours):
    for contour in contours:
        area = cv2.contourArea(contour)
        if area > 50:
            x, y, w, h = cv2.boundingRect(contour)
            cv2.rectangle(contour_img, (x, y), (x + w, y + h), (0, 255, 0), 2)

def limpar_arquivos_antigos():
    arquivos_antigos = [arquivo for arquivo in os.listdir('test') if arquivo.startswith('img_cropped')]
    for arquivo in arquivos_antigos:
        os.remove(os.path.join('test', arquivo))

def processar_imagem(nome_arquivo_imagem):
    img = cv2.imread(nome_arquivo_imagem) 
    
    if img is None:
        print('Erro ao carregar a imagem.')
        return
    
    segmented_img = flood_fill_segmentation(img, (0, 0))
    contours = find_contours(segmented_img)
    img_with_rectangles = img.copy()
    draw_rectangles(img_with_rectangles, contours)

    limpar_arquivos_antigos()

    contours_sorted = sorted(contours, key=lambda c: cv2.boundingRect(c)[0])

    for i, contour in enumerate(contours_sorted):
        x, y, w, h = cv2.boundingRect(contour)
        
        cropped_img = img[y:y+h, x:x+w]

        border_size = 50
        cropped_img_with_border = cv2.copyMakeBorder(cropped_img, border_size, border_size, border_size, border_size, cv2.BORDER_CONSTANT, value=(255, 255, 255))

        cropped_img_with_border_resized = cv2.resize(cropped_img_with_border, (128, 128))

        cv2.imwrite(f'test/img_cropped{i}.png', cropped_img_with_border_resized)

    # cv2.imshow('Imagem com retangulos', img_with_rectangles)
    cv2.waitKey(0)
    cv2.destroyAllWindows()

def prever_imagem():
    img_dir = 'test/'

    predicted_word = ""

    class_names = { 
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

            predictions = model.predict(img_array)

            predicted_class_index = np.argmax(predictions)

            predicted_class_name = class_names[predicted_class_index]

            predicted_probability = predictions[0][predicted_class_index]
            
            predicted_word += predicted_class_name
            
            
    print("Palavra prevista:", predicted_word)       
    return predicted_word

# Corrigir a palavra prevista
# correcoes_palavra_prevista = corrigir_ortografia(predicted_word)
# if correcoes_palavra_prevista:
#     nova_palavra_prevista = correcoes_palavra_prevista[0]['correcao']
#     print("Palavra prevista corrigida:", nova_palavra_prevista)
# else:
#     print("Não foram encontradas correções para a palavra prevista.")
    
# print("Classe prevista:", predicted_class_name)
# print("Probabilidade:", predicted_probability)

# print("Palavra prevista:", predicted_word)

# print(predictions)

@app.route('/cv', methods=['POST'])
def predict():
    processar_imagem('test/imagem.png')
    palavra = prever_imagem()
    
    print(palavra)
    
    return palavra
    
if __name__ == '__main__':
    app.run(debug=True)
