import os
import cv2 as cv
import numpy as np

def aplicar_operacao(pasta_raiz):
    for root, dirs, files in os.walk(pasta_raiz):
        for file in files:
            if file.lower().endswith(('.png', '.jpg', '.jpeg')):
                filepath = os.path.join(root, file)
                img = cv.imread(filepath)
                
                if img is not None:                    
                    kernel_size_dilate = np.random.randint(2, 41)
                    img_dilated = cv.dilate(img, np.ones((kernel_size_dilate, kernel_size_dilate)))
                    cv.imwrite(os.path.join(root, 'dilated_' + file), img_dilated)
                    
                    kernel_size_erode = np.random.randint(2, 41)
                    img_eroded = cv.erode(img, np.ones((kernel_size_erode, kernel_size_erode)))
                    cv.imwrite(os.path.join(root, 'eroded_' + file), img_eroded)

aplicar_operacao('alfanumerico')
