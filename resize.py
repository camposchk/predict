import os
import cv2 as cv

def redimensionar_imagens(pasta_raiz):
    for root, dirs, files in os.walk(pasta_raiz):
        for file in files:
            if file.lower().endswith(('.png', '.jpg', '.jpeg')):
                filepath = os.path.join(root, file)
                
                img = cv.imread(filepath)
                
                if img is not None:
                    img_redimensionada = cv.resize(img, (128, 128))
                    
                    cv.imwrite(filepath, img_redimensionada)

redimensionar_imagens('alfanumerico')
