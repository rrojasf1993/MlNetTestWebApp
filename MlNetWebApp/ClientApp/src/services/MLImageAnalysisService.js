
export const uploadFiles = async (formData) => {
    var uploadRequest = await fetch('MLImageAPI/UploadFiles',
        {
            method: 'POST',
            body: formData
        }
    )
    return uploadRequest;
}

export const analyzeUploadedPhotos = async () => {
    var analysisRequest = await fetch('MLImageAPI/GetAnalysisResults', {method:'GET'});
    return analysisRequest;
}    
